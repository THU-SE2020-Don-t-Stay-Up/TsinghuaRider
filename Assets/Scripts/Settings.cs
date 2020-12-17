using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    public const string ITEM_CONFIG_PATH = "./Assets/itemsInfo.json";
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
    public static List<Item> items;
    public static string[] prefabPaths;
    /// <summary>
    /// 房间难度信息
    /// </summary>
    public static float difficulty;
    public static float difficultyStep;
    /// <summary>
    /// 时间信息
    /// </summary>
    public static bool gamePaused;
    public static float totalTime;

    public static GameObject GetPrefab(string name)
    {
        return Resources.Load($"Prefabs/{name}", typeof(GameObject)) as GameObject;
    }

    public static AudioClip GetAudioClip(string name)
    {
        return Resources.Load($"Music/{name}", typeof(AudioClip)) as AudioClip;
    }
}

public static class Utility
{
    public static T GetInterface<T>(GameObject gameObject) where T : class
    {
        if (!typeof(T).IsInterface)
        {
            return null;
        }
        var interfaces = gameObject.GetComponents<Component>().OfType<T>();
        if (interfaces.Count() == 0) return null;
        return interfaces.First();
    }
    public static Vector3 Rotate(Vector3 v, float angle)
    {
        Vector3 vector = new Vector3(0, 0);
        float radian = angle * Mathf.PI / 180;
        vector.x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        vector.y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return vector.normalized;
    }
}

public interface IInteract
{
    void InteractWith(GameObject gameObject);
}