using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 继承monobehaviour和IInteract，提供生成item、被角色捡起两个大功能。
/// 使用方法：搞一个itemPrefab上，挂上BoxCollider2D，RigidBody2D，Sprite Renderer和这个ItemAgent脚本。
/// </summary>
public class ItemAgent : MonoBehaviour,  IInteract
{
    /// <summary>
    /// 在指定位置放置指定物品
    /// </summary>
    /// <param name="position"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static ItemAgent GenerateItem(Vector3 position, Item item)
    {
        // 在position处生成一个Item
        Transform transform = Instantiate(ItemAssets.Instance.ItemPrefab, position, Quaternion.identity);
        ItemAgent itemAgent = transform.GetComponent<ItemAgent>();
        itemAgent.SetItem(item);

        return itemAgent;
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


    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    public void InteractWith(GameObject gameObject)
    {
        CharacterAgent character = gameObject.GetComponent<CharacterAgent>();
        character.InventoryAddItem(item);
        DestorySelf();
    }

}
