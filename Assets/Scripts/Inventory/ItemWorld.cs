using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Sockets;

/// <summary>
/// ItemWorld是显示在游戏世界中的item
/// </summary>
public class ItemWorld : MonoBehaviour
{
    /// <summary>
    /// 在指定位置放置指定物品
    /// </summary>
    /// <param name="position"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        // 在position处实例化一个ItemWorld
        Transform transform = Instantiate(ItemAssets.Instance.ItemWorldPrefab, position, Quaternion.identity);
        
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

    }

    /// <summary>
    /// 设置这个item以及它的图像
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if (item.amount > 1)
        {
            textMeshPro.SetText(item.amount.ToString());

        }
        else
        {
            textMeshPro.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}
