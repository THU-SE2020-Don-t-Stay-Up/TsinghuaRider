using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品类(Item)的实现
/// </summary>
[Serializable]
public class Item {
    public enum ItemType{
        Sword,
        HealthPotion,
        ManaPotion,
        Coin,
        Medkit,
    }

    public ItemType itemType;
    public int amount;

    /// <summary>
    /// 获取物品的图片，通过已经实例化的ItemAssets
    /// </summary>
    /// <returns></returns>
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return ItemAssets.Instance.swordSprite;  
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;  
            case ItemType.ManaPotion: return ItemAssets.Instance.manaPotionSprite;  
            case ItemType.Coin: return ItemAssets.Instance.coinSprite;  
            case ItemType.Medkit: return ItemAssets.Instance.medkitSprite;  
        }
    }

    /// <summary>
    /// 该种物品是否可以在背包中折叠显示
    /// </summary>
    /// <returns></returns>
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
            case ItemType.Coin:
                return true;
            case ItemType.Sword:
            case ItemType.Medkit:
                return false;
        }
    }

    public void UseItem(Item item, Character character)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:  
                Debug.Log("我回血啦，我nb了！");
                break;

            case Item.ItemType.ManaPotion:
                Debug.Log("我很有精神！");
                break;
        }
    }
}