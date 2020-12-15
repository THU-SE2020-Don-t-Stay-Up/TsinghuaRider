using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TradeSystem : MonoBehaviour
{
    public static TradeSystem instance;

    public Image goodsImage;
    public Text goodsNameText;
    public Text goodsPriceText;

    public Button buyButton;

    public Goods signalGoods;
    public Transform goodsSlotContainer;
    public Transform goodsSlotTemplate;
    public List<Goods> goodsInfo = new List<Goods>();

private void Awake()
    {
        instance = this;
        goodsSlotContainer = transform.Find("goodsSlotContainer");
        goodsSlotTemplate = goodsSlotContainer.Find("goodsSlotTemplate");

        goodsImage = transform.Find("goodsImage").GetComponent<Image>();
        goodsInfo.Add(new HealthPotionGoods { });
    }

    public void OpenTrade()
    {
        transform.DOScale(1, 0.3f);

        foreach(Transform child in goodsSlotContainer)
        {
            if (child == goodsSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int y = 0;
        float goodsSlotHeight = 10f;
        foreach (Goods goods in goodsInfo)
        {
            var goodsSlot = Instantiate(goodsSlotTemplate, goodsSlotContainer);
            RectTransform goodsSlotRecTransform =goodsSlot.GetComponent<RectTransform>();
            Click click = goodsSlot.GetComponent<Click>();
            click.goods = goods;
            goodsSlotRecTransform.gameObject.SetActive(true);
            goodsSlotRecTransform.anchoredPosition = new Vector2(0, y * goodsSlotHeight);
            Image image = goodsSlotRecTransform.Find("image").GetComponent<Image>();
            image.sprite = goods.GetSprite();
            y--;
        }

    }

    public void UpdateUI()
    {
        Debug.Log("♂");
    }

}
