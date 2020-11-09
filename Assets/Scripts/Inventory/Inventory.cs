using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory 
{

    public event EventHandler OnItemListChanged;

    private List<Item> itemList;

    public Inventory() 
    {
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.Sword, amount = 1});
        AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1});
        AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1});
        AddItem(new Item { itemType = Item.ItemType.Coin, amount = 180000});
        //Debug.Log("I have an INVENTORY!!");
        //Debug.Log(itemList.Count);
        
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

}
