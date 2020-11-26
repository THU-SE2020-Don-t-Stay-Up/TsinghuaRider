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
    protected GameObject target;

    protected float attackDeltaTime = 0;
    protected float roamingTime;
    protected float roamingDeltaTime;
    protected Vector3 startPosition;
    protected Vector3 roamingDirection;
    protected Vector3 movingDirection;
    public Skill AttackSkill => Monster.Skills[0];
    //public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        living = Global.monsters[monsterIndex].Clone() as Monster;
        living.CurrentHealth = living.MaxHealth;
        actualLiving = Monster.Clone() as Monster;
        print(Monster.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();
        roamingTime = Random.Range(0.5f, 0.8f);
        roamingDeltaTime = roamingTime;

        startPosition = transform.position;
        SetRandomDirection();

        actionState = ActionState.Roaming;
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
                Attack();
                if (target == null)
                    actionState = ActionState.Restarting;
                break;
            case ActionState.Restarting:
                Restart();
                break;
            default:
                break;
        }
        CheckState();
        SetRandomDirection();
        attackDeltaTime += Time.deltaTime;
        roamingDeltaTime += Time.deltaTime;
    }

    void FixedUpdate()
    {

    }

    protected void SetAttackDirection()
    {
        if (target != null)
        {
            actualLiving.AttackDirection = (target.transform.position - transform.position).normalized;
        }
    }
    protected bool AttackTarget()
    {
        if (actualLiving.AttackSpeed - attackDeltaTime < 0.01)
        {
            SetAttackDirection();
            AttackSkill.Perform(this, target.GetComponent<LivingBaseAgent>());
            //print("attack target");
            attackDeltaTime = 0;
            return true;
        }
        return false;
    }
    public bool Attack()
    {
        if (target == null)
            return true;
        SetMovingDirection(target.transform.position);
        rigidbody2d.velocity = movingDirection * actualLiving.MoveSpeed;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= actualLiving.AttackRadius)
        {
            if (AttackTarget())
                return true;
        }
        return false;
    }


    protected void SetRandomDirection()
    {
        if (roamingTime - roamingDeltaTime <= 0.01)
        {
            Vector3 RandomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            roamingDirection = RandomDirection;
            roamingDeltaTime = 0;
        }
    }

    protected void SetMovingDirection(Vector3 position)
    {
        if (roamingTime - roamingDeltaTime <= 0.01)
        {
            Vector3 direction = (position - transform.position).normalized;
            Vector3 moveDirection = (position - transform.position - direction * actualLiving.AttackRadius).normalized;
            movingDirection = (moveDirection + roamingDirection).normalized;
        }
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
        target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < Monster.ViewRadius)
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
    public new void Destroy()
    {
        SplitSkill splitSkill = Monster.Skills.FirstOrDefault(e => e.GetType() == new SplitSkill().GetType()) as SplitSkill;
        if (splitSkill != null)
        {
            splitSkill.Perform(this, null);
        }
        else
        {
            ItemAgent.GenerateItem(transform.position, new Coin { Amount = ActualMonster.Reward });
        }
        GameObject.Destroy(gameObject);
    }
}