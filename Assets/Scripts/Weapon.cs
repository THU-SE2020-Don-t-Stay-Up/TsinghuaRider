using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item
{
    public float AttackSpeed;
    public float AttackRadius;
    public float AttackAmount;
    public float AttackAngle;
    public GameObject bulletPrefab;
    public Vector3 handleOffset;
    public Vector3 attackOffset;

    public virtual void Attack(CharacterAgent user, Vector3 direction) { }

    public override void Use(CharacterAgent character)
    {
        if (character.WeaponPrefab != null)
        {
            character.WeaponColumnAddItem(character.WeaponPrefab.GetComponent<WeaponAgent>().Weapon.Clone() as Item);
            GameObject.Destroy(character.WeaponPrefab);
        }
        character.WeaponPrefab = GameObject.Instantiate(Global.GetPrefab(GetType().ToString()), character.transform.position + handleOffset, Quaternion.identity, character.transform);
    }

    protected virtual void ExtraEffect(LivingBaseAgent agent) { }
}


public class Sword : Weapon
{
    public Sword()
    {
        AttackSpeed = 3;
        AttackRadius = 2;
        AttackAmount = 100;
        AttackAngle = 120;
        IsStackable = false;
        Amount = 1;
    }

    //public override void Attack(CharacterAgent user, Vector3 direction)
    //{
    //    user.Animator.SetTrigger("Melee");
    //    IEnumerable<GameObject> targetObjects = user.GetAttackRangeObjects(user.transform.position, user.ActualCharacter.AttackDirection, user.ActualCharacter.AttackRadius * AttackRadius, AttackAngle, "Monster");
    //    foreach (var targetObject in targetObjects)
    //    {
    //        LivingBaseAgent targetAgent = targetObject.GetComponent<LivingBaseAgent>();
    //        targetAgent.ChangeHealth(-user.ActualCharacter.AttackAmount * AttackAmount);
    //        ExtraEffect(targetAgent);
    //    }
    //}

}

public class Saber1 : Weapon
{
    public Saber1()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class Saber2 : Weapon
{
    public Saber2()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class Saber3 : Weapon
{
    public Saber3()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class BlackExcalibur : Weapon
{
    public BlackExcalibur()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class Excalibur : Weapon
{
    public Excalibur()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class Faith : Weapon
{
    public Faith()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class GilgameshEa : Weapon
{
    public GilgameshEa()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class MasterSword : Weapon
{
    public MasterSword()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class VirtuousTreaty : Weapon
{
    public VirtuousTreaty()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class xianyu : Weapon
{
    public xianyu()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}


public class qingqing : Weapon
{
    public qingqing()
    {
        AttackSpeed = 1;
        AttackRadius = 2;
        AttackAmount = 10;
        AttackAngle = 120;
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
            ExtraEffect(targetAgent);
        }
    }

}

public class Gun : Weapon
{
    public Gun()
    {
        handleOffset = new Vector3 (0.5f, 0.3f, 0f);
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
        
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.WeaponPrefab.transform.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 10);
    }

}

public class EnergyGun : Weapon
{
    public EnergyGun()
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
        Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset +direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 10);
    }

}

public class ChargeGun : Weapon
{
    public ChargeGun()
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
        Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 10);
    }

}


public class Gatling : Weapon
{
    public Gatling()
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
        Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 10);
    }

}

public class Puella : Weapon
{
    public Puella()
    {
        handleOffset = new Vector3(0.9f, 0.9f, 0f);
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

        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.WeaponPrefab.transform.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 10);
    }

}

public class RathBuster : Weapon
{
    public RathBuster()
    {
        handleOffset = new Vector3(0.9f, 0.9f, 0f);
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

        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.WeaponPrefab.transform.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 10);
    }

}

public class RathGunlance : Weapon
{
    public RathGunlance()
    {
        handleOffset = new Vector3(0.9f, 0.9f, 0f);
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

        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.WeaponPrefab.transform.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 10);
    }

}