using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// 技能抽象类，增加新技能时继承此抽象类，实现Perform方法
/// </summary>
abstract public class Skill
{
    protected float Timer;
    protected MonsterAgent agent;
    protected LivingBaseAgent target;

    public virtual void Init(MonsterAgent agent)
    {
        this.Timer = 0;
        this.agent = agent;
    }
    public abstract bool Perform();
}

abstract public class AttackSkill : Skill
{
    /// <summary>
    /// 向指定方向攻击
    /// </summary>
    /// <param name="attackDirection">攻击方向</param>
    /// <param name="extraEffect">攻击附带的额外效果（lambda）</param>
    public abstract bool AttackAt(Vector3 attackDirection, Action<LivingBaseAgent> extraEffect);
}
/// <summary>
/// 远程武器攻击
/// </summary>
public class MissleAttackSkill : AttackSkill
{
    public override bool Perform()
    {
        Timer += Time.deltaTime;
        target = agent.target;
        if (target == null)
            return true;
        agent.MoveTo(target.transform.position);
        return Attack();
    }

    protected bool Attack(Action<LivingBaseAgent> extraEffect)
    {
        float distance = Vector3.Distance(agent.transform.position, target.transform.position);
        if (distance <= agent.ActualMonster.AttackRadius)
        {
            if (agent.living.AttackSpeed < Timer)
            {
                Vector3 attackDirection = agent.GetAttackDirection();
                AttackAt(attackDirection, extraEffect);
                Timer = 0;
                return true;
            }
        }
        return false;
    }
    public override bool AttackAt(Vector3 attackDirection, Action<LivingBaseAgent> extraEffect)
    {
        GameObject projectileObject = GameObject.Instantiate(agent.bulletPrefab, agent.transform.position + attackDirection * agent.GetComponent<BoxCollider2D>().size.x, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(agent, agent.actualLiving.AttackAmount, extraEffect);
        bullet.Shoot(attackDirection, agent.ActualMonster.BulletSpeed);
        return true;
    }

    protected virtual bool Attack() => Attack(null);
}



/// <summary>
/// 近战普攻技能
/// </summary>
public class MeleeAttackSkill : AttackSkill
{
    public override bool Perform()
    {
        Timer += Time.deltaTime;
        target = agent.target;
        if (target == null)
            return true;
        agent.MoveTo(target.transform.position);
        return Attack();
    }
    protected bool Attack(Action<LivingBaseAgent> extraEffect)
    {
        float distance = Vector3.Distance(agent.transform.position, target.transform.position);
        if (distance <= agent.ActualMonster.AttackRadius)
        {
            if (agent.living.AttackSpeed < Timer)
            {
                Vector3 attackDirection = agent.GetAttackDirection();
                AttackAt(attackDirection, extraEffect);
                Timer = 0;
                return true;
            }
        }
        return false;
    }

    public override bool AttackAt(Vector3 attackDirection, Action<LivingBaseAgent> extraEffect)
    {
        bool attackFlag = false;
        IEnumerable<GameObject> targetObjects = agent.GetAttackRangeObjects(agent.transform.position, attackDirection, agent.ActualMonster.AttackRadius, agent.ActualMonster.AttackAngle, "Player");
        foreach (var targetObject in targetObjects)
        {
            LivingBaseAgent targetAgent = targetObject.GetComponent<LivingBaseAgent>();
            targetAgent.ChangeHealth(-agent.ActualMonster.AttackAmount);
            extraEffect?.Invoke(target);
            attackFlag = true;
        }
        return attackFlag;
        
    }

    protected virtual bool Attack() => Attack(null);
}

/// <summary>
/// 近战普攻减速技能
/// </summary>
public class MeleeSlowAttackSkill : MeleeAttackSkill
{

    protected override bool Attack()
    {
        return base.Attack(target => target.actualLiving.State.AddStatus(new SlowState(), 2));
    }
}


/// <summary>
/// 分裂技能
/// </summary>
public class SplitSkill : Skill
{
    public int splitNum = 3;
    public override bool Perform()
    {
        GameObject prefab = Global.GetPrefab($"微{agent.living.Name}");
        for (int i = 0; i < splitNum; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0, 1.0f), Random.Range(0, 1.0f));
            GameObject.Instantiate(prefab, agent.transform.position + randomOffset, Quaternion.identity);
        }
        return true;
    }
}

public class FierceSkill : Skill
{
    protected float bloodLine;
    public override void Init(MonsterAgent agent)
    {
        base.Init(agent);
        bloodLine = agent.ActualMonster.BloodLine;
    }
    public override bool Perform()
    {
        if (agent.ActualMonster.CurrentHealth * 1.0f / agent.ActualMonster.MaxHealth <= bloodLine)
        {
            agent.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            agent.ActualMonster.State.AddStatus(new FierceState(), 1);
        }
        return true;
    }
}

public class LaserSkill : Skill
{
    protected Lasers lasers;
    protected bool finished;
    public override void Init(MonsterAgent agent)
    {
        base.Init(agent);
        BossAgent boss = agent as BossAgent;
        lasers = boss.GetLasers();
    }

    public override bool Perform()
    {
        return lasers.AutoPerform();
    }
}

public class BarrageSkill : Skill
{
    protected Barrage barrage;

    public override void Init(MonsterAgent agent)
    {
        base.Init(agent);
        BossAgent boss = agent as BossAgent;
        barrage = boss.GetBarrage();
    }

    public override bool Perform()
    {
        return barrage.Perform();
    }
}

public class ObstacleSkill : Skill
{
    public override bool Perform()
    {
        target = agent.target;
        Vector3 randomOffset = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        GameObject.Instantiate(agent.bulletPrefab, target.transform.position + randomOffset, Quaternion.identity);
        return true;
    }
}

public class MissleBleedAttackSkill : MissleAttackSkill
{
    protected override bool Attack()
    {
        return base.Attack(agent => agent.actualLiving.State.AddStatus(new BleedState(), 2));
    }
}
public class MeleeChargeAttackSkill : MeleeAttackSkill
{
    private bool chargeStartFlag;
    private Vector3 chargeDirection;
    private float chargeTime = 1;
    private float chargeTimer;
    private int speedTimes = 3;
    private bool attackFlag;
    private float beatDistance = 3;
    public override void Init(MonsterAgent agent)
    {
        base.Init(agent);
        chargeStartFlag = false;
        chargeTimer = 0;
        attackFlag = false;
    }
    public override bool Perform()
    {
        Timer += Time.deltaTime;
        target = agent.target;
        if (target == null)
            return true;
        agent.MoveTo(target.transform.position);
        if (agent.ActualMonster.AttackSpeed < Timer)
        {
            startChargeAttack();
            if (Charge())
            {
                EndChargeAttack();
                return true;
            }
        }
        return false;
    }
    public void startChargeAttack()
    {
        if (!chargeStartFlag)
        {
            chargeStartFlag = true;
            chargeDirection = agent.GetAttackDirection();
            agent.actualLiving.MoveSpeed = agent.living.MoveSpeed * speedTimes;
            agent.actualLiving.AttackRadius = 1.5f;
        }
    }
    
    public void EndChargeAttack()
    {
        Timer = 0;
        chargeTimer = 0;
        chargeStartFlag = false;
        attackFlag = false;
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed;
        agent.actualLiving.AttackRadius = agent.living.AttackRadius;
    }

    public bool Charge()
    {
        chargeTimer += Time.deltaTime;
        agent.MoveTowards(chargeDirection);
        if (!attackFlag)
        {
            if (AttackAt(chargeDirection, BeatBack))
            {
                attackFlag = true;
            }
        }
        if (chargeTime < chargeTimer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BeatBack(LivingBaseAgent target)
    {
        Vector3 beatPosition = target.transform.position + chargeDirection * beatDistance;
        RaycastHit2D dashHit = Physics2D.Raycast(target.rigidbody2d.position, chargeDirection, beatDistance, LayerMask.GetMask("Obstacle"));
        if (dashHit.collider != null)
        {
            beatPosition = dashHit.point;
        }
        //target.rigidbody2d.MovePosition(new Vector2(beatPosition.x, beatPosition.y));
        target.transform.position = beatPosition;
    }
}

