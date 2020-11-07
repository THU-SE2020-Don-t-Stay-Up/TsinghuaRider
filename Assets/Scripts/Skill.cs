using Debug = UnityEngine.Debug;
/// <summary>
/// 技能抽象类，增加新技能时继承此抽象类，实现Perform方法
/// </summary>
abstract public class Skill
{
    public abstract void Perform(LivingBaseAgent subject, LivingBaseAgent target);
}

/// <summary>
/// 普攻技能
/// </summary>
public class AttackSkill : Skill
{
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
        target.ChangeHealth(-subject.living.AttackAmount);
        Debug.Log($"{subject.living.Name} attack {target.living.Name}, {target.living.Name} currentHealth is {target.living.CurrentHealth}");
    }
}

/// <summary>
/// 分裂技能
/// </summary>
public class SplitSkill : Skill
{
    public override void Perform(LivingBaseAgent subject, LivingBaseAgent target)
    {
    }
}
