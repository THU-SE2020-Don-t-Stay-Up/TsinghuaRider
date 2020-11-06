﻿using System;
using System.Collections.Generic;


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
}

/// <summary>
/// 状态变量，按bit存储状态信息
/// </summary>
[Flags]
public enum Status
{
    Normal = 0x0,
    Slow = 0x1,
    Poison = 0x2,
    Cold = 0x4,
    Fierce = 0x8,
    Invincible = 0x10
}
/// <summary>
/// 状态类，存储并操作object的状态
/// </summary>
public class State
{
    Status status = 0x00;
    /// <summary>
    /// 记录各种状态的持续时间
    /// </summary>
    public Dictionary<Status, float> StateDuration { get; } = new Dictionary<Status, float>();

    /// <summary>
    /// 添加某种状态
    /// </summary>
    /// <param name="status">待添加状态</param>
    public void AddStatus(Status status, float duration)
    {
        if(HasStatus(status))
        {
            StateDuration[status] += duration;
        }
        else
        {
            this.status |= status;
            StateDuration.Add(status, duration);
        }
    }
    /// <summary>
    /// 移除某种状态
    /// </summary>
    /// <param name="status">待删除状态</param>
    public void RemoveStatus(Status status)
    {
        if (HasStatus(status))
        {
            this.status &= ~status;
            StateDuration.Remove(status);
        }
    }
    /// <summary>
    /// 查询是否处于某种状态
    /// </summary>
    /// <param name="status">待查询状态</param>
    /// <returns>处于该状态返回true，不处于则返回false</returns>
    public bool HasStatus(Status status)
    {
        if ((this.status & status) != 0)
            return true;
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
        status = 0x00;
        StateDuration.Clear();
    }

}