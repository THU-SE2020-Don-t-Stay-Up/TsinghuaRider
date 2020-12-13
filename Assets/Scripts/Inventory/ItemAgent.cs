using TMPro;
using UnityEngine;

/// <summary>
/// 继承monobehaviour和IInteract，提供生成item、被角色捡起两个大功能。
/// 使用方法：搞一个itemPrefab上，挂上BoxCollider2D，RigidBody2D，Sprite Renderer和这个ItemAgent脚本。
/// </summary>
public class ItemAgent : MonoBehaviour, IInteract
{
    public int itemIndex;


    /// <summary>
    /// 在position处生成一个Item
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <param name="item">要生成的物品</param>
    public static void GenerateItem(Vector3 position, Item item)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Vector3 generatePoint = position + randomOffset;
        while (Physics2D.OverlapCircle(generatePoint, 0.52f, LayerMask.GetMask("Obstacle")) != null)
        {
            randomOffset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            generatePoint = position + randomOffset;
        }
        GameObject realItem = Instantiate(item.GetItemPrefab(), generatePoint, Quaternion.identity);
        ItemAgent itemAgent = realItem.GetComponent<ItemAgent>();
        itemAgent.SetItem(item);
    }

    public static void GenerateItem(Vector3 position, Item item, float p)
    {
        float r = Random.Range(0.0f, 1.0f);
        if (r < p)
        {
            GenerateItem(position, item);
        }
    }
    public Item Item { get; set; }
    protected TextMeshPro textMeshPro;

    private void Awake()
    {
        Item = Global.items[itemIndex].Clone() as Item;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// 设置这个item的属性值以及显示它的数量
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        // 为了Edit Mode测试，加入Awake()，之后需要删除
        Awake();
        this.Item = item;

        if (item.Amount > 1)
        {
            textMeshPro.SetText(item.Amount.ToString());
        }
        else
        {
            textMeshPro.SetText("");
        }
    }

    public virtual void InteractWith(GameObject gameObject)
    {
        CharacterAgent character = gameObject.GetComponent<CharacterAgent>();
        if (character != null)
        {
            character.InventoryAddItem(Item);
            Destroy(this.gameObject);
        }
    }

}
