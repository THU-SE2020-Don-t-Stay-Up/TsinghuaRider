using System.Linq;
using UnityEngine;

public class BossAgent : MonsterAgent
{
    private Monster Boss => living as Monster;
    public Monster ActualBoss => actualLiving as Monster;
    public LineRenderer LineRenderer { get; set; }

    public float bloodLine = 0.4f;

    public Lasers lasers;

    public enum AttackState
    {
        Attack,
        Laser
    }

    /// <summary>
    /// 攻击顺序
    /// </summary>
    private AttackState[] attackOrder = { AttackState.Attack, AttackState.Attack, AttackState.Attack, AttackState.Laser };

    private AttackState attackState;
    public bool NextAttackStateFlag { get; set; }

    private int attackIndex = 0;
    void Start()
    {
        living = Global.monsters[monsterIndex].Clone() as Monster;
        actualLiving = Boss.Clone() as Monster;
        living.CurrentHealth = living.MaxHealth;
        print(Boss.Name);
        
        rigidbody2d = GetComponent<Rigidbody2D>();
        roamingTime = Random.Range(0.5f, 0.8f);
        roamingDeltaTime = roamingTime;

        startPosition = transform.position;
        SetRandomDirection();

        actionState = ActionState.Roaming;
        attackState = AttackState.Attack;
        NextAttackStateFlag = false;
        living.MissleWeapon.bulletPrefab = bulletPrefab;

        lasers = new Lasers(this, 3, 1f, 45, 30);
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
                if (!NextAttackStateFlag || target != null && (target.transform.position - transform.position).magnitude <= Boss.ViewRadius)
                {
                    switch(attackState)
                    {
                        case AttackState.Attack:
                            NextAttackStateFlag = Attack();
                            break;
                        case AttackState.Laser:
                            NextAttackStateFlag = ShootLaser();
                            break;
                        default:
                            break;
                    }
                    if (NextAttackStateFlag && target != null)
                    {
                        attackState = NextAttackState();
                        NextAttackStateFlag = false;
                        lasers.ClearFlags();
                    }
                }
                else
                {
                    actionState = ActionState.Restarting;
                }
                break;
            case ActionState.Restarting:
                Restart();
                break;
            default:
                break;
        }
        Fierce();
        CheckState();
        SetRandomDirection();
        attackDeltaTime += Time.deltaTime;
        roamingDeltaTime += Time.deltaTime;
    }

    public AttackState NextAttackState()
    {
        attackIndex = (attackIndex + 1) % attackOrder.Length;
        return attackOrder[attackIndex];
    }

    public bool ShootLaser()
    {
        rigidbody2d.velocity = Vector3.zero;
        return lasers.AutoPerform();
    }

    public void Fierce()
    {
        new FierceSkill().Perform(this, null);
    }
}

public class Lasers
{
    public Laser[] lasers;
    public bool[] laserFlags;
    public float laserInternal;
    public Lasers(LivingBaseAgent agent, int laserNum, float laserInternal, float laserScope, float thetaSpeed)
    {
        lasers = new Laser[laserNum];
        laserFlags = new bool[laserNum];
        this.laserInternal = laserInternal;
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i] = new Laser(agent, i, laserScope, thetaSpeed);
            laserFlags[i] = false;
        }
    }
    public void ClearFlags()
    {
        for(int i = 0; i < lasers.Length; i++)
        {
            laserFlags[i] = false;
        }
    }
    public bool AutoPerform()
    {
        bool flag = true;
        for(int i = 0; i < lasers.Length; i++)
        {
            if (!laserFlags[i])
            {
                laserFlags[i] = lasers[i].AutoPerform(i * laserInternal);
            }
            flag = flag && laserFlags[i];
        }
        return flag;
    }
}

public class Laser
{
    /// <summary>
    /// 发射激光主体
    /// </summary>
    public LivingBaseAgent Agent { get; set; }
    public LineRenderer LineRenderer { get; set; }
    public bool LaserStartFlag { get; set; }
    /// <summary>
    /// 激光初始方向
    /// </summary>
    public Vector3 LaserStartDirection { get; set; }
    public Vector3 LaserDirection { get; set; }
    /// <summary>
    /// 用于计算激光伤害
    /// </summary>
    public float LaserTime { get; set; }
    public float LaserDeltaTime { get; set; }
    /// <summary>
    /// 激光角度
    /// </summary>
    public float LaserAngle { get; set; }
    /// <summary>
    /// 激光转速
    /// </summary>
    public float ThetaSpeed { get; set; }
    /// <summary>
    /// 激光范围
    /// </summary>
    public float LaserScope { get; set; }
    /// <summary>
    /// 射线检测
    /// </summary>
    public RaycastHit2D hit;

    public float Timer;
    public Laser(LivingBaseAgent agent, int laserIndex, float laserScope, float thetaSpeed)
    {
        Agent = agent;
        LaserScope = laserScope;
        ThetaSpeed = thetaSpeed;
        LineRenderer = agent.transform.GetChild(laserIndex).GetComponent<LineRenderer>();
        LaserStartFlag = false;
        LineRenderer.startWidth = 5;
        LineRenderer.endWidth = 5;
    }

    public void StartLaser()
    {
        if (!LaserStartFlag)
        {
            LaserStartFlag = true;
            LaserAngle = -LaserScope;
            LaserStartDirection = Agent.living.AttackDirection;
        }
    }
    public void EndLaser()
    {
        if (LaserStartFlag && !LineRenderer.enabled)
        {
            LaserStartFlag = false;
            LineRenderer.enabled = false;
            Timer = 0;
        }
    }

    public void GetNextAngle()
    {
        LaserAngle += Time.deltaTime * ThetaSpeed;
        if (LaserAngle <= LaserScope)
        {
            LaserDirection = Utility.Rotate(LaserStartDirection, LaserAngle);
        }
        else
        {
            LineRenderer.enabled = false;
        }
    }

    public bool IsStopped()
    {
        return !LineRenderer.enabled;
    }

    /// <summary>
    /// 在当前方向画出激光
    /// </summary>
    public void DrawLaser()
    {
        Vector3 firePosition = Agent.transform.position + LaserDirection * 2f;
        hit = Physics2D.Raycast(firePosition, LaserDirection, 100, LayerMask.GetMask("Obstacle", "Player"));
        if (hit && LineRenderer.enabled == true)//如果遇到障碍物且射线打开
        {
            LivingBaseAgent living = hit.transform.GetComponent<LivingBaseAgent>();
            if (living != null && !Agent.CompareTag(living.tag) && LaserTime - LaserDeltaTime < 0.01)
            {
                living.ChangeHealth(-Agent.actualLiving.AttackAmount * 2);
                LaserDeltaTime = 0;
            }
            LineRenderer.SetPosition(0, firePosition);
            LineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            LineRenderer.SetPosition(0, firePosition);
            LineRenderer.SetPosition(1, firePosition + LaserDirection * 100);
        }
    }

    public bool AutoPerform(float delay)
    {
        StartLaser();
        Timer += Time.deltaTime;
        if (Timer >= delay)
        {
            LineRenderer.enabled = true;
            GetNextAngle();
            DrawLaser();
            if (IsStopped())
            {
                EndLaser();
                return true;
            }
        }
        return false;
    }
}