using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// 在地图中生产初始物品
/// 脚本使用方法：在scene中创建一个gameobject，把这个脚本拖上去。
/// </summary>
public class ItemGenerator : MonoBehaviour
{
     public Item item;

    public void Awake()
    {
        ItemAgent.GenerateItem(new Vector3(2, 0), new HealthPotion { amount = 10, isStackable = true});
        ItemAgent.GenerateItem(new Vector3(-3, -4), new Medkit { amount = 1, isStackable = false});
        ItemAgent.GenerateItem(new Vector3(-3, -4), new Sword { amount = 1, isStackable = false});
        //ItemWorld.SpawnItemWorld(new Vector3(0, 2), new Item { itemType = Item.ItemType.Sword, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(-5, 5), new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(5, 5), new Item { itemType = Item.ItemType.Coin, amount = 10 });
        Debug.Log("Items generated!");
    }
}
