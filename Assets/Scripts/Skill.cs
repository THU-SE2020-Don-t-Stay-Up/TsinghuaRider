using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
/// <summary>
/// 技能抽象类，增加新技能时继承此抽象类，实现Perform方法
/// </summary>
abstract public class Skill
{
    public abstract void Perform(MonsterAgent subject, LivingBaseAgent target);
}

/// <summary>
/// 远程武器攻击
/// </summary>
public class MissleAttackSkill : Skill
{
    public override void Perform(MonsterAgent subject, LivingBaseAgent target)
    {
        GameObject projectileObject = GameObject.Instantiate(subject.bulletPrefab, subject.transform.position + subject.ActualMonster.AttackDirection * subject.GetComponent<BoxCollider2D>().size.x, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.StartPoint = subject.transform.position;
        bullet.Damage = subject.ActualMonster.AttackAmount;
        bullet.UserTag = subject.tag;
        bullet.Shoot(subject.ActualMonster.AttackDirection, subject.ActualMonster.BulletSpeed);
    }
}



/// <summary>
/// 近战普攻技能
/// </summary>
public class MeleeAttackSkill : Skill
{
    public override void Perform(MonsterAgent subject, LivingBaseAgent target)
    {
        IEnumerable<GameObject> targetObjects = subject.GetAttackRangeObjects(subject.transform.position, subject.ActualMonster.AttackDirection, subject.ActualMonster.AttackRadius, subject.ActualMonster.AttackAngle, "Player");
        foreach (var targetObject in targetObjects)
        {
            LivingBaseAgent targetAgent = targetObject.GetComponent<LivingBaseAgent>();
            targetAgent.ChangeHealth(-subject.ActualMonster.AttackAmount);
        }
    }
}

/// <summary>
/// 近战普攻减速技能
/// </summary>
public class MeleeSlowAttackSkill : Skill
{
    public override void Perform(MonsterAgent subject, LivingBaseAgent target)
    {
        new MeleeAttackSkill().Perform(subject, target);
        target.actualLiving.State.AddStatus(new SlowState(), 2);
    }
}


/// <summary>
/// 分裂技能
/// </summary>
public class SplitSkill : Skill
{
    public int splitNum = 3;
    public override void Perform(MonsterAgent subject, LivingBaseAgent target)
    {
        GameObject prefab = Global.GetPrefab($"微{subject.living.Name}");
        for(int i = 0; i < splitNum; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0, 1.0f), Random.Range(0, 1.0f));
            GameObject.Instantiate(prefab, subject.transform.position + randomOffset, Quaternion.identity);
            Debug.Log($"分裂{i}");
        }
    }
}

public class FierceSkill : Skill
{
    public override void Perform(MonsterAgent subject, LivingBaseAgent target)
    {
        BossAgent boss = subject as BossAgent;
        if (boss.living.CurrentHealth * 1.0f / boss.living.MaxHealth <= boss.bloodLine)
        {
            subject.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            boss.living.State.AddStatus(new FierceState(), 1);
        }
    }
}

public class ObstacleSkill : Skill
{
    public override void Perform(MonsterAgent subject, LivingBaseAgent target)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        GameObject.Instantiate(subject.bulletPrefab, target.transform.position + randomOffset, Quaternion.identity);
    }
}

public class MeleeDashAttackSkill : Skill
{
    public override void Perform(MonsterAgent subject, LivingBaseAgent target)
    {
    }
}