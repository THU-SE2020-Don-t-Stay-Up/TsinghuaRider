using Newtonsoft.Json;
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

    public abstract bool Use(CharacterAgent character);

    public GameObject GetItemPrefab()
    {
        return Global.GetPrefab("Inventory&Items/" + this.ToString());
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

    public float Recovery { get; set; } = 0.1f;

    public HealthPotion()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }
    public override bool Use(CharacterAgent character)
    { 
        if (character.actualLiving.CurrentHealth < character.actualLiving.MaxHealth)
        {
            character.ChangeHealth(Recovery * character.ActualCharacter.MaxHealth);
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class StrengthPotion : Item
{
    public float AdditionalStrength { get; set; } = 0.2f;
    public StrengthPotion()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }
    public override bool Use(CharacterAgent character) 
    { 
        character.ActualCharacter.AttackAmount *= (1 + AdditionalStrength);
        return true;
    }
}

public class Coin : Item
{
    public Coin()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }
    public override bool Use(CharacterAgent character ) { return true; }

}


public class Medkit : Item
{
    public float Recovery { get; set; } = 0.7f;

    public Medkit()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }

    public override bool Use(CharacterAgent character)
    {
        if (character.actualLiving.CurrentHealth < character.actualLiving.MaxHealth)
        {
            character.ChangeHealth(Recovery * character.ActualCharacter.MaxHealth);
            return true;
        }
        else
        {
            return false;
        }
    }
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