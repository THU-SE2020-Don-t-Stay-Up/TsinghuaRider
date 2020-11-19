using TMPro;
using UnityEngine;

/// <summary>
/// 继承monobehaviour和IInteract，提供生成item、被角色捡起两个大功能。
/// 使用方法：搞一个itemPrefab上，挂上BoxCollider2D，RigidBody2D，Sprite Renderer和这个ItemAgent脚本。
/// </summary>
public class ItemAgent : MonoBehaviour, IInteract
{
    public int ItemIndex;


    /// <summary>
    /// 在position处生成一个Item
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <param name="item">要生成的物品</param>
    public static void GenerateItem(Vector3 position, Item item)
    {
        GameObject itemPrefab = ItemAssets.GetItemPrefab(item);
        GameObject realItem = Instantiate(itemPrefab, position, Quaternion.identity);
        ItemAgent itemAgent = realItem.GetComponent<ItemAgent>();
        itemAgent.SetItem(item);
    }


    public Item Item { get; set; }
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        Item = ItemAssets.Instance.items[ItemIndex];
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// 设置这个item的属性值以及显示它的数量
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        this.Item = item;

        if (item.amount > 1)
        {
            textMeshPro.SetText(item.amount.ToString());
        }
        else
        {
            textMeshPro.SetText("");
        }
    }

    public void InteractWith(GameObject gameObject)
    {
        CharacterAgent character = gameObject.GetComponent<CharacterAgent>();
        character.InventoryAddItem(Item);
        Destroy(this.gameObject);
    }

}
