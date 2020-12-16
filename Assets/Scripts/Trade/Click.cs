using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour, IPointerClickHandler
{
    public Goods goods;
    public void OnPointerClick(PointerEventData eventData)
    {
        TradeSystem.instance.signalGoods = goods;
        TradeSystem.instance.UpdateUI();
    }
}
