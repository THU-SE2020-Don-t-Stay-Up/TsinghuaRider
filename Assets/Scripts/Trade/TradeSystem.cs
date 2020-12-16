using System.Collections;
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
    public List<Goods> goodsInfo = new List<Goods>();

    public Transform goodsScrollView;
    public Transform goodsTemplate;

private void Awake()
    {
        instance = this;

        goodsScrollView = transform.Find("goodsScrollView");
        goodsTemplate =  goodsScrollView.Find("Viewport").Find("Content").Find("goods");

        goodsImage = transform.Find("goodsImage").GetComponent<Image>();
        goodsNameText = transform.Find("goodsName").GetComponent<Text>();
        goodsDescriptionText = transform.Find("goodsDescription").GetComponent<Text>();
        goodsPriceText = transform.Find("goodsPrice").GetComponent<Text>();

        GotoTop();

        goodsInfo.Add(new HealthPotionGoods { });
        goodsInfo.Add(new StrengthPotionGoods { });
        goodsInfo.Add(new MedkitGoods{ });
        goodsInfo.Add(new BlackExcaliburGoods { });
        goodsInfo.Add(new ExcaliburGoods { });
        goodsInfo.Add(new FaithGoods { });
        goodsInfo.Add(new GilgameshEaGoods { });
        goodsInfo.Add(new MasterSwordGoods { });
        goodsInfo.Add(new VirtuousTreatyGoods { });
        goodsInfo.Add(new xianyuGoods { });
        goodsInfo.Add(new qingqingGoods { });
        goodsInfo.Add(new EnergyGunGoods { });
        goodsInfo.Add(new ChargeGunGoods { });
        goodsInfo.Add(new GatlingGoods { });
        goodsInfo.Add(new PuellaGoods { });
        goodsInfo.Add(new RathBusterGoods { });
        goodsInfo.Add(new RathGunlanceGoods { });

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

        GotoTop();

        foreach (Transform child in goodsScrollView.Find("Viewport").Find("Content"))
        {
            if (child == goodsTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Goods goods in goodsInfo)
        {
            var goodsSlot = Instantiate(goodsTemplate, goodsScrollView.Find("Viewport").Find("Content"),false);
            goodsSlot.transform.DOScale(1, 0.3f);
            RectTransform goodsSlotRecTransform = goodsSlot.GetComponent<RectTransform>();
            Click click = goodsSlot.GetComponent<Click>();
            click.goods = goods;
            goodsSlotRecTransform.gameObject.SetActive(true);
            Image image = goodsSlotRecTransform.Find("image").GetComponent<Image>();
            image.sprite = goods.GetSprite();
        }
        GotoTop();

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

    private void GotoTop()
    {
        RectTransform contentRecTransform = goodsScrollView.Find("Viewport").Find("Content").GetComponent<RectTransform>();
        var localPos = contentRecTransform.localPosition;
        localPos.y = 0;
        contentRecTransform.localPosition = localPos;
    }

}
