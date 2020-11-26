﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine;

/// <summary>
/// 提供item基类以及具体物品类
/// </summary>
abstract public class Item:ICloneable
{
    public bool IsStackable { get; set; }
    public int Amount { get; set; }
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
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return GetType().ToString();
    }

    public abstract void Use(CharacterAgent character);

    public GameObject GetItemPrefab()
    {
        return Global.GetPrefab(this.ToString());
    }

    public Sprite GetSprite()
    {
        return GetItemPrefab().GetComponent<SpriteRenderer>().sprite;
    }
    public static void SaveItem(List<Item> items)
    {
        string path = Settings.ITEM_CONFIG_PATH;
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new ItemsJsonConverter());
        using (StreamWriter sw = new StreamWriter(path))
        {
            JsonSerializer.Create(settings).Serialize(new JsonTextWriter(sw), items);
        }
    }

    public static List<Item> LoadItem()
    {
        string path = Settings.ITEM_CONFIG_PATH;
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new ItemsJsonConverter());
        List<Item> items = JsonSerializer.Create(settings).Deserialize<List<Item>>(new JsonTextReader(new StreamReader(path)));
        return items;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}


public class HealthPotion : Item
{
    public int Recovery { get; set; } = 2;

    public HealthPotion()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }
    public override void Use(CharacterAgent character) { character.ChangeHealth(Recovery); }
}

public class StrengthPotion : Item
{
    public int AdditionalStrength { get; set; } = 4;
    public StrengthPotion()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }
    public override void Use(CharacterAgent character) { character.living.AttackAmount += AdditionalStrength; }
}

public class Coin : Item
{
    public Coin()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }
    public override void Use(CharacterAgent character) { }

}


public class Medkit : Item
{
    public int Recovery { get; set; } = 11;

    public Medkit()
    {
        this.IsStackable = false;
        this.Amount = 1;
    }

    public override void Use(CharacterAgent character) { character.ChangeHealth(Recovery); }
}
//修改了Weapon中的部分内容；试图整合了原来Item与ItemAsset中的内容

class ItemsJsonConverter : JsonConverter<List<Item>>
{
    public override List<Item> ReadJson(JsonReader reader, Type objectType, List<Item> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (existingValue == null) existingValue = new List<Item>();
        JArray arr = JArray.Load(reader);
        foreach (var s in arr)
        {          
            existingValue.Add((Item)Activator.CreateInstance(typeof(Item).Assembly.GetType(s.ToString())));
        }
        return existingValue;
    }

    public override void WriteJson(JsonWriter writer, List<Item> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        if (value != null)
        {
            foreach (var s in value)
            {
                string name = s.GetType().Name;
                writer.WriteValue(name);
            }
        }
        writer.WriteEndArray();
    }
}