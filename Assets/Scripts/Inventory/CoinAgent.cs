using System;
using TMPro;
using UnityEngine;

public class CoinAgent : ItemAgent
{
    private void Awake()
    {
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }
    public override void InteractWith(GameObject gameObject)
    {
        CharacterAgent character = gameObject.GetComponent<CharacterAgent>();
        if (character != null)
        {
            character.CoinInventoryAddItem(Item);
            Destroy(this.gameObject);
        }
    }
}
