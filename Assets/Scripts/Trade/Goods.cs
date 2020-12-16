using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Goods : Item
{
    public Sprite RealImage { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PriceText { get; set; }

    public int price;

    public override bool Use(CharacterAgent character) { return true; }

    public virtual void Buy(CharacterAgent character) { }
}

public class HealthPotionGoods: Goods
{
    public HealthPotionGoods()
    {
        this.Name = "Health Potion";
        this.Description = "Recover 10% of maximun health.";
        this.PriceText = "300";
        this.price = 300;

        this.RealImage = new HealthPotion { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price)
        {
            character.InventoryAddItem(new HealthPotion { });
            for(int i = 0; i < price; i++)
            {
                character.UseMoney();
            }
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class StrengthPotionGoods: Goods
{
    public StrengthPotionGoods()
    {
        this.Name = "Strength Potion";
        this.Description = "Improve your attack to 120%.";
        this.PriceText = "1500";
        this.price = 1500;

        this.RealImage = new StrengthPotion { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price)
        {
            character.InventoryAddItem(new StrengthPotion { });
            for (int i = 0; i < price; i++)
            {
                character.UseMoney();
            }
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class MedkitGoods : Goods
{
    public MedkitGoods()
    {
        this.Name = "Medkit";
        this.Description = "Recover 70% of maximun health.";
        this.PriceText = "1500";
        this.price = 1500;

        this.RealImage = new Medkit { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price)
        {
            character.InventoryAddItem(new Medkit { });
            for (int i = 0; i < price; i++)
            {
                character.UseMoney();
            }
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}
