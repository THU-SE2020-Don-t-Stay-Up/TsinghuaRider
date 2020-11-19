using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例化游戏中所有item，主要是给itemPrefab提供不同的图标以供选择
/// 脚本使用方法：在scene中创建一个gameobject，把这个脚本拖上去，把已经设置好的item的prefab拖上去
/// </summary>
public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        foreach (var itemName in Global.itemNames)
        {
            items.Add((Item)Activator.CreateInstance(typeof(Item).Assembly.GetType(itemName)));
        }
    }

    public List<Item> items = new List<Item>();
    public GameObject[] itemPrefabs;

    public static GameObject GetItemPrefab(Item item)
    {
        int index = Instance.items.FindIndex(e => e.Equals(item));
        return Array.Find(Instance.itemPrefabs, e => e.GetComponent<ItemAgent>().ItemIndex == index);
    }

    public static Sprite GetSprite(Item item)
    {
        return GetItemPrefab(item).GetComponent<SpriteRenderer>().sprite;
    }
}
