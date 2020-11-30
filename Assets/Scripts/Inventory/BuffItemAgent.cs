using System;
using TMPro;
using UnityEngine;

public class BuffItemAgent :ItemAgent
{
    public Item BuffItem { get; set; }

    private void Awake()
    {
        BuffItem = Global.items[itemIndex].Clone() as Item;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public override void InteractWith(GameObject gameObject)
    {
        CharacterAgent character = gameObject.GetComponent<CharacterAgent>();
        if (character != null)
        {
            character.BuffColumnAddItem(Item);
            BuffItem.Use(character);
            Destroy(this.gameObject);
        }
    }

}
