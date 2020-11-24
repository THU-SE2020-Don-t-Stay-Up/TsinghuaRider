using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 背包类(Inventory)的实现
/// </summary>
public class Inventory 
{
    /// <summary>
    /// 这个OnItemListChanged用来记录每一次背包内物品的变动
    /// </summary>
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    //private Action<Item> useItemAction;

    /// <summary>
    /// 初始化背包，传入使用方法。带有一些物品
    /// </summary>
    /// <param name="useItemAction"></param>
    public Inventory() 
    {

        itemList = new List<Item>();
        AddItem(new HealthPotion {  Amount = 3 });
        AddItem(new StrengthPotion {  Amount = 4 });
        AddItem(new Medkit { Amount = 1 });

        //Debug.Log("I have an INVENTORY!!");
        //Debug.Log(itemList.Count);

    }

    /// <summary>
    /// 用来处理背包内物品的增加，并且记录在事件中。
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if (item.IsStackable)
        {
            var inventoryItem = itemList.Find(e => e.Equals(item));
            if (inventoryItem == null)
            {
                itemList.Add(item);
            }
            else
            {
                inventoryItem.Amount += item.Amount;
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool RemoveItem(Item item)
    {
        bool itemFlag = true;

        var inventoryItem = itemList.Find(e => e.Equals(item));
        if (inventoryItem != null)
        {
            inventoryItem.Amount -= item.Amount;
            if (inventoryItem.Amount <= 0)
            {
                itemList.Remove(inventoryItem);
            }
        }
        else
        {
            itemFlag = false;
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return itemFlag;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void UseItem(Item item, CharacterAgent character)
    {
        if (RemoveItem(item))
        {
            item.Use(character);
        }
        else
        {
            Debug.Log("无了啊！");
        }
    }

}
