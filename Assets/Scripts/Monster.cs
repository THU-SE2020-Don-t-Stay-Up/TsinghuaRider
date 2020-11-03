using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Reflection;
using UnityEngine;



public class Monster
{   
    public string Name { get; set; }
    public float MoveSpeed { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int Defense { get; set; }
    /// <summary>
    /// 韧性，减少控制时长
    /// </summary>
    public float Tenacity { get; }
    public float AttackSpeed { get; set; }
    public float ViewAngle { get; set; }
    public float ViewRadius { get; set; }
    public float AttackRadius { get; set; }
    public string TexturePath { get; }
    public int Reward { get; set; }
    public int Difficulty { get; set; }
    [JsonConverter(typeof(SkillsJsonConverter))]
    public List<Skill> Skills { get; set; }
    public Status Status { get; set; }

    /// <summary>
    /// 加载怪物配置文件，游戏初始化时调用
    /// </summary>
    /// <returns>monster list</returns>
    public static List<Monster> LoadMonster()
    {
        string path = Settings.MONSTER_CONFIG_PATH;
        List<Monster> monsters = JsonSerializer.CreateDefault().Deserialize<List<Monster>>(new JsonTextReader(new StreamReader(path)));
        return monsters;
    }

    public static void SaveMonster(List<Monster> monsters)
    {
        string path = Settings.MONSTER_CONFIG_PATH;
        using (StreamWriter sw = new StreamWriter(path))
        {
            JsonSerializer.CreateDefault().Serialize(new JsonTextWriter(sw), monsters);
        }
    }

}

class SkillsJsonConverter : JsonConverter<List<Skill>>
{
    public override List<Skill> ReadJson(JsonReader reader, Type objectType, List<Skill> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (existingValue == null) existingValue = new List<Skill>();
        JArray arr = JArray.Load(reader);
        foreach (var s in arr)
        {
            existingValue.Add((Skill)Activator.CreateInstance(typeof(Skill).Assembly.GetType(s.ToString() + "Skill")));
        }
        return existingValue;
    }

    public override void WriteJson(JsonWriter writer, List<Skill> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        if (value != null)
        {
            foreach (var s in value)
            {
                string name = s.GetType().Name;
                writer.WriteValue(name.Substring(0, name.Length - 5));
            }
        }
        writer.WriteEndArray();
    }
}

