using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

public class StateLoader
{
    [JsonConverter(typeof(TJsonConverter<StateBase>))]
    public static List<StateBase> States { get; set; }

    public static List<StateBase> LoadStates()
    {
        string path = Settings.STATE_CONFIG_PATH;
        List<StateBase> states = new List<StateBase>();
        JsonReader reader = new JsonTextReader(new StreamReader(path));
        JArray arr = JArray.Load(reader);
        foreach (var s in arr)
        {
            states.Add((StateBase)Activator.CreateInstance(typeof(StateBase).Assembly.GetType(s.ToString())));
        }
        return states;
    }
}
public class SlowState : StateBase
{
    private float SlowFactor;
    public SlowState(float slowFactor = 0.2f)
    {
        SlowFactor = slowFactor;
    }
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

public class SpeedState : StateBase
{
    private float factor;
    public SpeedState(float Factor)
    {
        factor = Factor;
    }

    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed * factor;
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.MoveSpeed = agent.living.MoveSpeed;
    }
}

public class AttackSpeedState : StateBase
{
    float factor;
    public AttackSpeedState(float Factor)
    {
        factor = Factor;
    }

    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.AttackSpeed = agent.living.AttackSpeed * factor;
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.AttackSpeed = agent.living.AttackSpeed;
    }
}

public class AgilityState : StateBase
{
    private float factor;
    public AgilityState(float Factor)
    {
        factor = Factor;
    }

    public override void Effect(LivingBaseAgent agent)
    {
        MonsterAgent monsterAgent = agent as MonsterAgent;
        monsterAgent.ActualMonster.Agility = Global.monsters[monsterAgent.monsterIndex].Agility * factor;
    }

    public override void Resume(LivingBaseAgent agent)
    {
        MonsterAgent monsterAgent = agent as MonsterAgent;
        monsterAgent.ActualMonster.Agility = Global.monsters[monsterAgent.monsterIndex].Agility;
    }
}

public class HealthState : StateBase
{
    private float factor;
    private bool used;
    public HealthState(float Factor)
    {
        factor = Factor;
        used = false;
    }

    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.MaxHealth = (int)(agent.living.MaxHealth * factor);
        if (!used)
        {
            agent.RestoreHealth();
            used = true;
            Debug.Log("Max Health: " + agent.actualLiving.MaxHealth);
            Debug.Log("Current Health: " + agent.actualLiving.CurrentHealth);
        }
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.MaxHealth = agent.living.MaxHealth;
        agent.actualLiving.CurrentHealth = (int)(agent.actualLiving.CurrentHealth / factor);
    }
}

public class InfestedState : StateBase
{
    public GameObject infested;
    public int number;
    public InfestedState(int Number)
    {
        number = Number;
        infested = Global.GetPrefab("MonsterGenerators/融合卷怪");
    }

    public override void Effect(LivingBaseAgent agent)
    {
        // 检测有无state
    }

    public override void Resume(LivingBaseAgent agent)
    {
        // 检测有无state
    }
}

public class AttackAmountState : StateBase
{
    private float factor;
    public AttackAmountState(float Factor)
    {
        factor = Factor;
    }

    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.AttackAmount = agent.living.AttackAmount * factor;
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.AttackAmount = agent.living.AttackAmount;
    }
}

public class AttackRadiusState : StateBase
{
    private float factor;
    public AttackRadiusState(float Factor)
    {
        factor = Factor;
    }

    public override void Effect(LivingBaseAgent agent)
    {
        agent.actualLiving.AttackRadius = agent.living.AttackRadius * factor;
    }

    public override void Resume(LivingBaseAgent agent)
    {
        agent.actualLiving.AttackRadius = agent.living.AttackRadius;
    }
}

public class BleedState: StateBase
{
    private float Timer = 0;
    private float DamagePerSecond;
    public BleedState(float damagePerSecond = 1.0f)
    {
        DamagePerSecond = damagePerSecond;
    }
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

    public StateBase GetState(StateBase status)
    {
        return StateDuration.Keys.ToArray().FirstOrDefault(e => e.GetType() == status.GetType());
    }
}