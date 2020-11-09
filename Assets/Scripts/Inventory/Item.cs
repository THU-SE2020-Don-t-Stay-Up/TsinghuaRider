using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}