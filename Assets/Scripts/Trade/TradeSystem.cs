﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TradeSystem : MonoBehaviour
{
    public static TradeSystem instance;

    GameObject mahou;
    GameObject robot;
    CharacterAgent character;

    public Image goodsImage;
    public Text goodsNameText;
    public Text goodsPriceText;
    public Text goodsDescriptionText;

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
        goodsNameText = transform.Find("goodsName").GetComponent<Text>();
        goodsDescriptionText = transform.Find("goodsDescription").GetComponent<Text>();
        goodsPriceText = transform.Find("goodsPrice").GetComponent<Text>();

        goodsInfo.Add(new HealthPotionGoods { });
        goodsInfo.Add(new StrengthPotionGoods { });
        goodsInfo.Add(new MedkitGoods{ });

        robot = GameObject.Find("RobotPrefab");
        mahou = GameObject.Find("MahouPrefab");

        if (UISelectCharacter.characterIndex == 0)
        {
            character = mahou.GetComponent<CharacterAgent>();
        }
        else
        {
            character = robot.GetComponent<CharacterAgent>();
        }


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
        float goodsSlotHeight = 35f;
        foreach (Goods goods in goodsInfo)
        {
            var goodsSlot = Instantiate(goodsSlotTemplate, goodsSlotContainer);
            RectTransform goodsSlotRecTransform =goodsSlot.GetComponent<RectTransform>();
            Click click = goodsSlot.GetComponent<Click>();
            click.goods = goods;
            goodsSlotRecTransform.gameObject.SetActive(true);
            goodsSlotRecTransform.anchoredPosition = new Vector2(0, y * goodsSlotHeight - 40f);
            Image image = goodsSlotRecTransform.Find("image").GetComponent<Image>();
            image.sprite = goods.GetSprite();
            y--;
        }
        goodsImage.enabled = false;
        goodsNameText.text = "";
        goodsDescriptionText.text = "";
        goodsPriceText.text = "";
    }

    public void UpdateUI()
    {
        goodsImage.enabled = true;
        goodsImage.sprite = signalGoods.RealImage;
        goodsNameText.text = signalGoods.Name;
        goodsDescriptionText.text = signalGoods.Description;
        goodsPriceText.text = signalGoods.PriceText;

        Debug.Log("♂");
    }

    public void CloseTrade()
    {
        transform.DOScale(0, 0.3f);
    }

    public void Buy()
    {
        signalGoods.Buy(character);
    }
}