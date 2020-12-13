using System;
using TMPro;
using UnityEngine;

public class CoinAgent : ItemAgent
{
    public Item Coin { get; set; }

    private void Awake()
    {
        Coin = Global.items[itemIndex].Clone() as Item;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public override void InteractWith(GameObject gameObject)
    {
        CharacterAgent character = gameObject.GetComponent<CharacterAgent>();
        if (character != null)
        {
            character.CoinInventoryAddItem(Coin);
            Destroy(this.gameObject);
        }
    }
}
