using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Goods : Item
{
    public Sprite RealImage { get; set; }


    public override bool Use(CharacterAgent character) { return true; }

}

public class HealthPotionGoods: Goods
{
    public string discription = "Recover 10% of maximun health.";
    public string name = "Health Potion";
    public int price = 10;
    public HealthPotionGoods()
    {
        this.RealImage= new HealthPotion { }.GetSprite();
    }

}
