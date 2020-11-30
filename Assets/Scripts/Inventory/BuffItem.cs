using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleItem : Item
{
    public InvincibleItem()
    {
        this.IsStackable = true;
        this.Amount = 1;
    }
    public override void Use(CharacterAgent character)
    {
        character.living.State.AddStatus(new InvincibleState(), float.NaN);
    }
}
