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

    public List<Item> ItemList { get; private set; }
    //private Action<Item> useItemAction;

    /// <summary>
    /// 初始化背包，传入使用方法。带有一些物品
    /// </summary>
    /// <param name="useItemAction"></param>
    public Inventory() 
    {
        ItemList = new List<Item>();
    }

    /// <summary>
    /// 用来处理背包内物品的增加，并且记录在事件中。
    /// 规定：武器均为unstackable，其他均为stackable
    /// 如果item添加之前不在背包中，返回false，否则返回true
    /// </summary>
    /// <param name="item"></param>
    public bool AddItem(Item item)
    {
        bool isInInventory;
        var inventoryItem = ItemList.FirstOrDefault(e => e.Equals(item));
        if (inventoryItem == null)
        {
            isInInventory = false;
            ItemList.Add(item);
        }
        else
        {
            isInInventory = true;
            if (item.IsStackable)
            {
                inventoryItem.Amount += item.Amount;
            }
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return isInInventory;
    }

    public void RemoveItem(Item item)
    {
        var inventoryItem = ItemList.FirstOrDefault(e => e.Equals(item));
        inventoryItem.Amount -= item.Amount;
        if (inventoryItem.Amount <= 0)
        {
            ItemList.Remove(inventoryItem);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool HasItem(Item item)
    {
        var inventoryItem = ItemList.FirstOrDefault(e => e.Equals(item));
        if (inventoryItem != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseItem(Item item, CharacterAgent character)
    {
        if (HasItem(item))
        {
            item.Use(character);
            RemoveItem(item);
        }
        else
        {
            Debug.Log("无了啊！");
        }
    }

}
