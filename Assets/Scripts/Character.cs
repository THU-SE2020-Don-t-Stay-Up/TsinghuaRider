using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// 角色属性类，继承自LivingBase，存储角色所有基本属性
/// </summary>
public class Character : LivingBase, ICloneable
{
    /// <summary>
    /// 魔力值
    /// </summary>
    public int MaxMagic { get; set; } = 5;
    public int CurrentMagic { get; set; }

    
    /// <summary>
    /// 加载人物配置文件，游戏初始化时调用
    /// </summary>
    /// <returns>character list</returns>
    public static List<Character> LoadCharacter()
    {
        string path = Settings.CHARACTER_CONFIG_PATH;
        List<Character> characters = JsonSerializer.CreateDefault().Deserialize<List<Character>>(new JsonTextReader(new StreamReader(path)));
        return characters;
    }

    public static void SaveCharacter(List<Character> characters)
    {
        string path = Settings.CHARACTER_CONFIG_PATH;
        using (StreamWriter sw = new StreamWriter(path))
        {
            JsonSerializer.CreateDefault().Serialize(new JsonTextWriter(sw), characters);
        }
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
