using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public float AttackSpeed;
    public float AttackRadius;
    public float AttackAmount;
    public float AttackAngle;
    public GameObject bulletPrefab;
    public virtual void Attack(CharacterAgent user, Vector3 direction) { }

    public override void Use(CharacterAgent character)
    {
        character.InventoryAddItem(character.WeaponPrefab.GetComponent<WeaponAgent>().Weapon.Clone() as Item);
        var oldWeapon = character.WeaponPrefab;
        WeaponAgent.Use(character, this);
        GameObject.Destroy(oldWeapon);
    }
}


public class Sword : Weapon
{
    public Sword()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 45;
        IsStackable = false;
        Amount = 1;
    }

    public override void Attack(CharacterAgent user, Vector3 direction)
    {
        user.Animator.SetTrigger("Melee");
        IEnumerable<GameObject> targetObjects = user.GetAttackRangeObjects(user.transform.position, user.ActualCharacter.AttackDirection, user.ActualCharacter.AttackRadius * AttackRadius, AttackAngle, "Monster");
        foreach (var targetObject in targetObjects)
        {
            LivingBaseAgent targetAgent = targetObject.GetComponent<LivingBaseAgent>();
            targetAgent.ChangeHealth(-user.ActualCharacter.AttackAmount * AttackAmount);
        }
    }

}

public class Gun : Weapon
{
    public Gun()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 20;
        AttackAmount = 10;
        AttackAngle = 0;
        IsStackable = false;
        AttackAmount = 8;
        Amount = 1;
    }

    public override void Attack(CharacterAgent user, Vector3 direction)
    {
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, this);
        bullet.Shoot(direction, 10);
    }

}