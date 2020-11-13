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

    float deltaTime = 0;
    Vector3 startPosition;
    Vector3 roamingPosition;

    public Skill AttackSkill => living.Skills[0];
    //public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        living = Global.monsters[monsterIndex];
        print(Monster.Name);

        rigidbody2d = GetComponent<Rigidbody2D>();

        

        startPosition = transform.position;
        roamingPosition = GetRoamPosition();

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
                MoveToPosition(roamingPosition);
                if (HasArrived(roamingPosition))
                    roamingPosition = GetRoamPosition();
                FindTarget();
                break;
            case ActionState.Chasing:
                if (target != null)
                {
                    MoveToPosition(target.transform.position);
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
                MoveToPosition(startPosition);
                if (HasArrived(startPosition))
                    actionState = ActionState.Roaming;
                break;
            default:
                break;
        }
        deltaTime += Time.deltaTime;
    }

    void FixedUpdate()
    {

    }

    void MoveToPosition(Vector3 targetPosition)
    {
        rigidbody2d.velocity = Vector3.zero;
        Vector3 direction = Vector3.Normalize(targetPosition - transform.position);
        if (Vector3.Distance(transform.position, targetPosition) >= living.AttackRadius)
            transform.Translate(MoveSpeed * direction * Time.deltaTime);
    }

    void AttackTarget()
    {
        if (living.AttackSpeed - deltaTime < 0.01)
        {
            living.AttackDirection = target.transform.position - transform.position;
            AttackSkill.Perform(this, target.GetComponent<LivingBaseAgent>());
            //print("attack target");
            deltaTime = 0;
        }
    }

    Vector3 GetRoamPosition()
    {
        Vector3 RandomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        //print("Roming to " + RandomDirection);
        return startPosition + RandomDirection * Random.Range(10f, 20f);

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


    //void ApplyStatus()
    //{
    //    if (living.State.HasStatus(Status.Slow))
    //    {
    //        MoveSpeed = living.MoveSpeed * 0.5f;
    //    }
    //    else
    //    {
    //        MoveSpeed = living.MoveSpeed;
    //    }

    //    if (living.State.HasStatus(Status.Cold))
    //    {
    //        MoveSpeed = 0;
    //    }
    //    else
    //    {
    //        MoveSpeed = living.MoveSpeed;
    //    }
    //}
}