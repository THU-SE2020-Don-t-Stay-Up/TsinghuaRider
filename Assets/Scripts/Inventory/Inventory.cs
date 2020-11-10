using System;
using System.Collections;
using System.Collections.Generic;
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
    private Action<Item> useItemAction;

    /// <summary>
    /// 初始化背包，传入使用方法。带有一些物品
    /// </summary>
    /// <param name="useItemAction"></param>
    public Inventory(Action<Item> useItemAction) 
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.Sword, amount = 1});
        AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1});
        AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1});
        AddItem(new Item { itemType = Item.ItemType.Coin, amount = 180});
        //Debug.Log("I have an INVENTORY!!");
        //Debug.Log(itemList.Count);
        
    }

    /// <summary>
    /// 用来处理背包内物品的增加，并且记录在事件中。
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
           if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

}
