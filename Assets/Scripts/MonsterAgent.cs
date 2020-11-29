using System.Linq;
using UnityEngine;

public class MonsterAgent : LivingBaseAgent
{
    /// <summary>
    /// 怪物基本属性信息
    /// </summary>
    protected Monster Monster => living as Monster;

    public Monster ActualMonster => actualLiving as Monster;
    /// <summary>
    /// 怪物编号，用于加载对应怪物的属性信息
    /// </summary>
    public int monsterIndex;
    public GameObject bulletPrefab;
    /// <summary>
    /// 状态机
    /// </summary>
    protected enum ActionState
    {
        Roaming,
        Chasing,
        Restarting
    };
    protected ActionState actionState;

    /// <summary>
    /// 攻击目标，默认为玩家
    /// </summary>
    public LivingBaseAgent target { get; set; }

    protected float roamingTime;
    protected float roamingTimer;
    protected Vector3 startPosition;
    protected Vector3 roamingDirection;
    protected Vector3 movingDirection;
    protected int SkillIndex { get; set; }

    //public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        living = Global.monsters[monsterIndex].Clone() as Monster;
        actualLiving = Monster.Clone() as Monster;
        actualLiving.CurrentHealth = actualLiving.MaxHealth;
        print(Monster.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();
        //roamingTime = 2;
        roamingTime = Random.Range(0.5f, 0.8f);
        roamingTimer = roamingTime;

        startPosition = transform.position;
        SetRandomDirection();

        actionState = ActionState.Roaming;

        SkillIndex = 0;
        ActualMonster.SkillOrder.ForEach(skill => skill.Init(this));
        ActualMonster.Skills.ForEach(skill => skill.Init(this));

        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (actionState)
        {
            case ActionState.Roaming:
                Roaming();
                break;
            case ActionState.Chasing:
                if (target != null && (target.transform.position - transform.position).magnitude <= Monster.ViewRadius)
                {
                    SetMovingDirection(target.transform.position);
                    rigidbody2d.velocity = movingDirection * ActualMonster.MoveSpeed;
                }
                else
                {
                    actionState = ActionState.Restarting;
                    ActualMonster.SkillOrder[SkillIndex].EndSkill(this);
                    SkillIndex = 0;
                }
                if (ActualMonster.SkillOrder[SkillIndex].Perform(this))
                    SkillIndex = (SkillIndex + 1) % ActualMonster.SkillOrder.Count;
                break;
            case ActionState.Restarting:
                Restart();
                break;
            default:
                break;
        }
        CheckState();
        SetRandomDirection();
        roamingTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {

    }


    protected void SetRandomDirection()
    {
        if (roamingTime < roamingTimer)
        {
            Vector3 RandomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            roamingDirection = RandomDirection;
            roamingTimer = 0;
        }
    }

    protected void SetMovingDirection(Vector3 position)
    {
        if (roamingTime < roamingTimer)
        {
            Vector3 direction = (position - transform.position).normalized;
            Vector3 moveDirection = (position - transform.position - direction * actualLiving.AttackRadius).normalized;
            movingDirection = (moveDirection + roamingDirection).normalized;
        }
    }

    public override Vector3 GetAttackDirection()
    {
        if (target != null)
        {
            return (target.transform.position - transform.position).normalized;
        }
        return Vector3.zero;
    }
    public void MoveTo(Vector3 position)
    {
        SetMovingDirection(position);
        rigidbody2d.velocity = movingDirection * actualLiving.MoveSpeed;
    }
    public void MoveTowards(Vector3 direction)
    {
        rigidbody2d.velocity = direction * actualLiving.MoveSpeed;
    }
    protected void Roaming()
    {
        rigidbody2d.velocity = roamingDirection * actualLiving.MoveSpeed;
        if ((transform.position - startPosition).magnitude > 100)
        {
            actionState = ActionState.Restarting;
        }
        FindTarget();
    }
    protected void Restart()
    {
        SetMovingDirection(startPosition);
        rigidbody2d.velocity = movingDirection * actualLiving.MoveSpeed;
        if (HasArrived(startPosition))
            actionState = ActionState.Roaming;
    }

    protected void FindTarget()
    {
        try
        {
            target = GameObject.FindWithTag("Player").GetComponent<LivingBaseAgent>();
        }
        catch (System.Exception)
        {
            target = null;
        }
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < Monster.ViewRadius && roamingTime < roamingTimer)
            {
                actionState = ActionState.Chasing;
            }
        }
    }

    protected bool HasArrived(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) < actualLiving.AttackRadius)
            return true;
        else
            return false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IInteract interact = Utility.GetInterface<IInteract>(collision.gameObject);
        if (interact != null)
        {
            interact.InteractWith(gameObject);
        }

    }
    public override void Destroy()
    {
        Skill splitSkill = Monster.Skills.FirstOrDefault(e => e is SplitSkill);
        if (splitSkill != null)
        {
            splitSkill.Perform(this);
        }
        else
        {
            ItemAgent.GenerateItem(transform.position, new Coin { Amount = ActualMonster.Reward });
        }
        base.Destroy();
    }
}