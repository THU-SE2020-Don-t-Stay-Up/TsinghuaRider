using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    /// <summary>
    /// 每当背包内物品有变动事件时，就调用刷新背包UI的函数。（顺便一提，系统内置的Event处理机制真好用，都不要手动传参！）
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        //首先删除UI所有的非item模板项目，防止某物品消耗了还显示在UI上
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 50.0f;

        // 在UI上摆放物品，并显示其图片
        foreach (Item item in inventory.ItemList)
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.Amount > 1)
            {
                uiText.SetText(item.Amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }
            // 一行4个
            x++;
            if (x > 3)
            {
                x = 0;
                y--;
            }
        }
    }
}
