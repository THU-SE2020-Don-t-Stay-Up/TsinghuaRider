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
            character.UseMoney(price);
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
            character.UseMoney(price);
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
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}


// Weapon商品
public class BlackExcaliburGoods : Goods
{
    public BlackExcaliburGoods()
    {
        this.Name = "Black Excalibur";
        this.Description = "Powerful and evil.";
        this.PriceText = "1500";
        this.price = 1500;

        this.RealImage = new BlackExcalibur { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new BlackExcalibur { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class ExcaliburGoods : Goods
{
    public ExcaliburGoods()
    {
        this.Name = "Excalibur";
        this.Description = "Ex~~Calibur!!!";
        this.PriceText = "1400";
        this.price = 1400;

        this.RealImage = new Excalibur { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new Excalibur { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class FaithGoods : Goods
{
    public FaithGoods()
    {
        this.Name = "Faith";
        this.Description = "Your weak but faithful sword!";
        this.PriceText = "500";
        this.price = 500;

        this.RealImage = new Faith { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new Faith { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class GilgameshEaGoods : Goods
{
    public GilgameshEaGoods()
    {
        this.Name = "EA";
        this.Description = "DONT TOUCH IT";
        this.PriceText = "1500";
        this.price = 1500;

        this.RealImage = new GilgameshEa { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new GilgameshEa{ });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class MasterSwordGoods : Goods
{
    public MasterSwordGoods()
    {
        this.Name = "Master Sword";
        this.Description = "Your useful and powerful company!";
        this.PriceText = "1400";
        this.price = 1400;

        this.RealImage = new MasterSword { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new MasterSword { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class VirtuousTreatyGoods : Goods
{
    public VirtuousTreatyGoods()
    {
        this.Name = "Virtuous Treaty";
        this.Description = "Cherish it!";
        this.PriceText = "700";
        this.price = 700;

        this.RealImage = new VirtuousTreaty { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new VirtuousTreaty { });
            character.UseMoney(price);

        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class xianyuGoods : Goods
{
    public xianyuGoods()
    {
        this.Name = "xianyu";
        this.Description = "May be you can eat it.";
        this.PriceText = "1700";
        this.price = 1700;

        this.RealImage = new xianyu { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new xianyu { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class qingqingGoods : Goods
{
    public qingqingGoods()
    {
        this.Name = "QQ";
        this.Description = "May be you can eat it.";
        this.PriceText = "2000";
        this.price = 2000;

        this.RealImage = new qingqing { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new qingqing { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class EnergyGunGoods : Goods
{
    public EnergyGunGoods()
    {
        this.Name = "Energy Gun";
        this.Description = "Powerful and dangerous.";
        this.PriceText = "2000";
        this.price = 2000;

        this.RealImage = new EnergyGun { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new EnergyGun { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class ChargeGunGoods : Goods
{
    public ChargeGunGoods()
    {
        this.Name = "Charge Gun";
        this.Description = "Powerful and dangerous.";
        this.PriceText = "2000";
        this.price = 2000;

        this.RealImage = new ChargeGun { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new ChargeGun { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class GatlingGoods : Goods
{
    public GatlingGoods()
    {
        this.Name = "Gatling";
        this.Description = "Gatling is justice!";
        this.PriceText = "2000";
        this.price = 2000;

        this.RealImage = new Gatling { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new Gatling { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class PuellaGoods : Goods
{
    public PuellaGoods()
    {
        this.Name = "Puella";
        this.Description = "Just shoot!";
        this.PriceText = "2000";
        this.price = 2000;

        this.RealImage = new Puella { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new Puella { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class RathBusterGoods : Goods
{
    public RathBusterGoods()
    {
        this.Name = "Rath Buster";
        this.Description = "Just shoot!";
        this.PriceText = "2000";
        this.price = 2000;

        this.RealImage = new RathBuster { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new RathBuster { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}

public class RathGunlanceGoods : Goods
{
    public RathGunlanceGoods()
    {
        this.Name = "Rath Gunlance";
        this.Description = "Just shoot!";
        this.PriceText = "2000";
        this.price = 2000;

        this.RealImage = new RathGunlance { }.GetSprite();
    }

    public override void Buy(CharacterAgent character)
    {
        if (character.Money() >= price && character.CanAddWeapon())
        {
            character.WeaponColumnAddItem(new RathGunlance { });
            character.UseMoney(price);
        }
        else
        {
            Debug.Log("我没钱！");
        }
    }
}
