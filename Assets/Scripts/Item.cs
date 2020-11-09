using System.Collections.Generic;

public class Item
{
    /// <summary>
    /// 是否可堆叠
    /// </summary>
    public bool Compositable;
}

public struct ItemStack
{
    public Item item;
    public int amount;
}

public class BackPack
{
    public List<ItemStack> content;
    public void AddItemStack(ItemStack itemStack)
    {

    }

    public void RemoveItemStack(ItemStack itemStack)
    {

    }
}