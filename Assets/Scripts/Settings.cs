using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using System.Linq;
using UnityEngine;

class Settings
{
    /// <summary>
    /// 怪物属性信息配置文件路径
    /// </summary>
    public const string MONSTER_CONFIG_PATH = "./Assets/monstersInfo.json";
    /// <summary>
    /// 角色属性信息配置文件路径
    /// </summary>
    public const string CHARACTER_CONFIG_PATH = "./Assets/charactersInfo.json";
}

/// <summary>
/// 全局变量
/// </summary>
class Global
{
    /// <summary>
    /// 所有怪物属性信息
    /// </summary>
    public static List<Monster> monsters;
    /// <summary>
    /// 所有角色属性信息
    /// </summary>
    public static List<Character> characters;
    public static string[] itemNames = { "HealthPotion", "StrengthPotion", "Coin", "Medkit", "Sword" };

}

/// <summary>
/// 状态类，存储状态Effect方法
/// </summary>
abstract public class StateBase
{
    public abstract void Effect(LivingBaseAgent agent);

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

public class NormalState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
        //Debug.Log("我很正常");
    }
}

public class SlowState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
        //Debug.Log("我减速了");
    }
}

public class InvincibleState : StateBase
{
    public override void Effect(LivingBaseAgent agent)
    {
        //Debug.Log("我无敌了");
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
        if (HasStatus(status))
        {
            StateDuration[status] += duration;
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
        if(StateDuration.ContainsKey(status))
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

public static class Utility
{
    public static T GetInterface<T>(GameObject gameObject) where T: class
    {
        if (!typeof(T).IsInterface)
        {
            return null;
        }
        var interfaces = gameObject.GetComponents<Component>().OfType<T>();
        if (interfaces.Count() == 0) return null;
        return interfaces.First();
    }
}

public interface IInteract
{
    void InteractWith(GameObject gameObject);
}