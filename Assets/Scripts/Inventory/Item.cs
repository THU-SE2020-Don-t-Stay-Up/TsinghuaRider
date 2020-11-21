using UnityEngine;

/// <summary>
/// 提供item基类以及具体物品类
/// </summary>
abstract public class Item
{
    public int amount { get; set; }
    public bool isStackable { get; set; }

    public override bool Equals(object obj)
    {

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return GetType().ToString();
    }

    public abstract void Use(CharacterAgent character);

    public Sprite GetSprite()
    {
        return ItemAssets.GetSprite(this);
    }
    public bool IsStackable() { return isStackable; }

}


public class HealthPotion : Item
{
    public int recovery = 2;

    //public override Sprite GetSprite()
    //{
    //    return ItemAssets.Instance.healthPotionSprite;
    //}
    public override void Use(CharacterAgent character) { character.ChangeHealth(recovery); }
}

public class StrengthPotion : Item
{
    public int additional_strength = 2;
    public override void Use(CharacterAgent character) { character.living.AttackAmount += additional_strength; }
}

public class Coin : Item
{
    public override void Use(CharacterAgent character) { }

}

public class Sword : Item
{
    public override void Use(CharacterAgent character) { }

}

public class Medkit : Item
{
    public int recovery = 10;

    public override void Use(CharacterAgent character) { character.ChangeHealth(recovery); }
}
//修改了Weapon中的部分内容；试图整合了原来Item与ItemAsset中的内容

