using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
/// <summary>
/// 技能抽象类，增加新技能时继承此抽象类，实现Perform方法
/// </summary>
abstract public class Skill
{
    public abstract void Perform(LivingBaseAgent subject, LivingBaseAgent target);
}

/// <summary>
/// 远程武器攻击
/// </summary>
public class MissleAttackSkill : Skill
{
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
        subject.living.MissleWeapon.Attack(subject.gameObject, subject.living.AttackDirection);
    }
}



/// <summary>
/// 近战普攻技能
/// </summary>
public class MeleeAttackSkill : Skill
{
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
        subject.living.MeleeWeapon.Attack(subject.gameObject, subject.living.AttackDirection);
    }
}

/// <summary>
/// 近战普攻减速技能
/// </summary>
public class MeleeSlowAttackSkill : Skill
{
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
        subject.living.MeleeWeapon.Attack(subject.gameObject, subject.living.AttackDirection);
        //Debug.Log($"{target.living.Name}获得减速效果");
        target.living.State.AddStatus(new SlowState(), 2);
    }
}


/// <summary>
/// 分裂技能
/// </summary>
public class SplitSkill : Skill
{
    public int splitNum = 3;
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
        GameObject prefab = Global.GetPrefab($"微{subject.living.Name}");
        for(int i = 0; i < splitNum; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0, 1.0f), Random.Range(0, 1.0f));
            GameObject little = GameObject.Instantiate(prefab, subject.transform.position + randomOffset, Quaternion.identity);
            Debug.Log($"分裂{i}");
        }
    }
}
