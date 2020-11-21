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
/// 近战普攻技能
/// </summary>
public class MissleAttackSkill : Skill
{
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
        subject.living.MissleWeapon.Attack(subject.gameObject, subject.living.AttackDirection);
    }
}

/// <summary>
/// 远程武器攻击
/// </summary>
public class MeleeAttackSkill : Skill
{
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
        subject.living.MeleeWeapon.Attack(subject.gameObject, subject.living.AttackDirection);
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
        GameObject prefab = subject.gameObject;
        for(int i = 0; i < splitNum; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0, 1.0f), Random.Range(0, 1.0f));
            GameObject little = GameObject.Instantiate(prefab, prefab.transform.position + randomOffset, Quaternion.identity);
            little.transform.localScale /= 2;
            LivingBase littleLiving = little.GetComponent<LivingBaseAgent>().living;
            littleLiving.MaxHealth /= 2;
            littleLiving.AttackAmount /= 2;
            littleLiving.Skills.Remove(new SplitSkill());
        }
    }
}
