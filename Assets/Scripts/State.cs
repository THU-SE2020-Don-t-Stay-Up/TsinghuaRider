using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 状态类，存储状态Effect方法
/// </summary>
abstract public class StateBase
{
    public abstract void Effect(LivingBaseAgent agent);

    public abstract void Resume(LivingBaseAgent agent);

    // override object.Equals
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }

    public override string ToString()
    {
        return GetType().ToString();
    }
}

public class SlowState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed * 0.2f;
        //Debug.Log("我减速了");
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed;
    }
}

public class InvincibleState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
        //Debug.Log("我无敌了");
    }
    public override void Resume(LivingBaseAgent agent)
    {
    }
}

public class FierceState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed * 1.5f;
        agent.actualLiving.AttackAmount = agent.living.AttackAmount * 2;
        agent.actualLiving.AttackSpeed = agent.living.AttackSpeed * 2f;
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed;
        agent.actualLiving.AttackAmount = agent.living.AttackAmount;
        agent.actualLiving.AttackSpeed = agent.living.AttackSpeed;
    }
}

public class SpeedUpState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed * 2.0f;
        agent.actualLiving.AttackRadius = 2.0f;
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.actualLiving.MoveSpeed;
        agent.actualLiving.AttackRadius = agent.living.AttackRadius;
    }
}

public class BleedState: StateBase
{
    private float Timer = 0;
    private float DamagePerSecond = 1.0f;
    public override void Effect(LivingBaseAgent agent)
    {
        Timer += Time.deltaTime;
        if (Timer >= 1.0f)
        {
            agent.ChangeHealth(-DamagePerSecond);
            Timer = 0;
        }
    }

    public override void Resume(LivingBaseAgent agent)
    {
    }
}

public class VertigoState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
    }

    public override void Resume(LivingBaseAgent agent)
    {
    }
}

/// <summary>
/// 状态类，存储并操作object的状态
/// </summary>
public class State
{
    /// <summary>
    /// 记录各种状态的持续时间
    /// </summary>
    public Dictionary<StateBase, float> StateDuration { get; } = new Dictionary<StateBase, float>();

    /// <summary>
    /// 添加某种状态
    /// </summary>
    /// <param name="status">待添加状态</param>
    public void AddStatus(StateBase status, float duration)
    {
        if (duration <= 0)
        {
            return;
        }
        if (HasStatus(status))
        {
            if (StateDuration[status] < duration || float.IsNaN(duration))
                StateDuration[status] = duration;
        }
        else
        {
            StateDuration.Add(status, duration);
        }
    }
    /// <summary>
    /// 移除某种状态
    /// </summary>
    /// <param name="status">待删除状态</param>
    public void RemoveStatus(StateBase status)
    {
        if (HasStatus(status))
        {
            StateDuration.Remove(status);
        }
    }
    /// <summary>
    /// 查询是否处于某种状态
    /// </summary>
    /// <param name="status">待查询状态</param>
    /// <returns>处于该状态返回true，不处于则返回false</returns>
    public bool HasStatus(StateBase status)
    {
        if (StateDuration.ContainsKey(status))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 清空所有状态
    /// </summary>
    public void ClearStatus()
    {
        StateDuration.Clear();
    }

}