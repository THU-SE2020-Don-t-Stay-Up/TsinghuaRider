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
        character.actualLiving.State.AddStatus(new InvincibleState(), float.NaN);
        //character.ActualCharacter.TimeInvincible = 3f;
    }
}
