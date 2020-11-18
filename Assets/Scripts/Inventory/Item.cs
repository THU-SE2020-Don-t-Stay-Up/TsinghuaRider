using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提供item基类以及具体物品类
/// </summary>
abstract public class Item
{
    public string itemType { get; set; }//Item可细分为MileeWeapon,MissleWeapon,HealthPotion,StrengthPotion,Coin
    public int amount { get; set; }
    public bool isStackable { get; set; }

    public abstract void Use(CharacterAgent character);

    public abstract Sprite GetSprite();
    public bool IsStackable() { return isStackable; }

}


public class HealthPotion : Item
{
    public int recovery = 2;

    public override Sprite GetSprite()
    {
        return ItemAssets.Instance.healthPotionSprite;
    }
    public override void Use(CharacterAgent character) { character.ChangeHealth(recovery); }
}

public class StrengthPotion : Item
{
    public int additional_strength = 2;

    public override Sprite GetSprite()
    {
        return ItemAssets.Instance.strengthPotionSprite;
    }

    public override void Use(CharacterAgent character) { character.living.AttackAmount += additional_strength; }
}

public class Coin : Item { 
    public override void Use(CharacterAgent character) { }
    public override Sprite GetSprite()
    {
        return ItemAssets.Instance.coinSprite;
    }

}

public class Medkit: Item
{
    public int recovery = 10;

    public override Sprite GetSprite()
    {
        return ItemAssets.Instance.medkitSprite;
    }
    public override void Use(CharacterAgent character) { character.ChangeHealth(recovery); }
}
//修改了Weapon中的部分内容；试图整合了原来Item与ItemAsset中的内容

