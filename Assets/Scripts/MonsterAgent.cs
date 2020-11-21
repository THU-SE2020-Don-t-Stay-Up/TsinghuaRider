using UnityEngine;

public class MonsterAgent : LivingBaseAgent
{
    /// <summary>
    /// 怪物基本属性信息
    /// </summary>
    private Monster Monster => living as Monster;
    /// <summary>
    /// 怪物编号，用于加载对应怪物的属性信息
    /// </summary>
    public int monsterIndex;
    /// <summary>
    /// 状态机
    /// </summary>
    enum ActionState
    {
        Roaming,
        Chasing,
        Restarting
    };
    private ActionState actionState;

    /// <summary>
    /// 攻击目标，默认为玩家
    /// </summary>
    GameObject target;

    float attackDeltaTime = 0;
    float roamingTime;
    float roamingDeltaTime;
    Vector3 startPosition;
    Vector3 roamingDirection;
    Vector3 movingDirection;
    public Skill AttackSkill => living.Skills[0];
    //public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        living = Global.monsters[monsterIndex].Clone() as Monster;
        print(Monster.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();
        roamingTime = Random.Range(0.5f, 0.8f);
        roamingDeltaTime = roamingTime;

        startPosition = transform.position;
        SetRandomDirection();

        MoveSpeed = living.MoveSpeed;
        actionState = ActionState.Roaming;
        living.MissleWeapon.bulletPrefab = bulletPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        switch (actionState)
        {
            case ActionState.Roaming:
                rigidbody2d.velocity = roamingDirection * MoveSpeed;
                if ((transform.position - startPosition).magnitude > 100)
                {
                    actionState = ActionState.Restarting;
                }
                FindTarget();
                break;
            case ActionState.Chasing:
                if (target != null)
                {
                    SetMovingDirection(target.transform.position);
                    rigidbody2d.velocity = movingDirection * MoveSpeed;
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= living.AttackRadius)
                    {
                        AttackTarget();
                    }
                    else if (distance > Monster.ViewRadius)
                    {
                        actionState = ActionState.Restarting;
                    }
                }
                else
                {
                    actionState = ActionState.Restarting;
                }
                break;
            case ActionState.Restarting:
                SetMovingDirection(startPosition);
                rigidbody2d.velocity = movingDirection * MoveSpeed;
                if (HasArrived(startPosition))
                    actionState = ActionState.Roaming;
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

    void AttackTarget()
    {
        if (living.AttackSpeed - attackDeltaTime < 0.01)
        {
            living.AttackDirection = (target.transform.position - transform.position).normalized;
            AttackSkill.Perform(this, target.GetComponent<LivingBaseAgent>());
            //print("attack target");
            attackDeltaTime = 0;
        }
    }



    void SetRandomDirection()
    {
        if (roamingTime - roamingDeltaTime <= 0.01)
        {
            Vector3 RandomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            roamingDirection = RandomDirection;
            roamingDeltaTime = 0;
        }
    }

    void SetMovingDirection(Vector3 position)
    {
        if (roamingTime - roamingDeltaTime <= 0.01)
        {
            Vector3 direction = (position - transform.position).normalized;
            Vector3 moveDirection = (position - transform.position - direction * living.AttackRadius).normalized;
            movingDirection = (moveDirection + roamingDirection).normalized;
        }
    }

    void FindTarget()
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

    bool HasArrived(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) < living.AttackRadius)
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

}