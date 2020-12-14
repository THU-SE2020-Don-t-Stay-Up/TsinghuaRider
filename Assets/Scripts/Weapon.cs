using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item
{
    public float AttackSpeed;
    public float AttackRadius;
    public float SwordLightRadius=5.0f;
    public float AttackAmount;
    public float AttackAngle;
    public GameObject bulletPrefab;
    public Vector3 handleOffset;
    public Vector3 attackOffset;
    public Vector3 offset;

    public virtual void Attack(CharacterAgent user, Vector3 direction) { }
    public virtual void SwordLightAttack(CharacterAgent user, Vector3 direction) { }

    public override bool Use(CharacterAgent character)
    {

        if (character.WeaponPrefab != null)
        {
            character.WeaponColumnAddItem(character.WeaponPrefab.GetComponent<WeaponAgent>().Weapon.Clone() as Item);
            GameObject.Destroy(character.WeaponPrefab);
        }
        GameObject weaponsition = GameObject.Find("weaponPosition");
        character.WeaponPrefab = GameObject.Instantiate(Global.GetPrefab(GetType().ToString()), weaponsition.transform.position, Quaternion.identity, character.transform);

        return true;
    }

    protected virtual void ExtraEffect(LivingBaseAgent agent) { }
}


public class Sword : Weapon
{
    public Sword()
    {
        AttackSpeed = 0.05f;
        AttackRadius = 2;
        AttackAmount = 2;
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
        AttackSpeed = 0.05f;
        AttackRadius = 2;
        AttackAmount = 5;
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

    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 1, ExtraEffect, SwordLightRadius);//刀光伤害为1
        bullet.Shoot(direction, 10);
    }

}

public class Saber2 : Weapon
{
    public Saber2()
    {
        AttackSpeed = 0.05f;
        AttackRadius = 2;
        AttackAmount = 5;
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
    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 1, ExtraEffect, SwordLightRadius);//刀光伤害为1
        bullet.Shoot(direction, 10);
    }
}

public class Saber3 : Weapon
{
    public Saber3()
    {
        AttackSpeed = 0.05f;
        AttackRadius = 2;
        AttackAmount = 5;
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

    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 1, ExtraEffect, SwordLightRadius);//刀光伤害为1
        bullet.Shoot(direction, 10);
    }

}

public class BlackExcalibur : Weapon
{
    public BlackExcalibur()
    {
        AttackSpeed = 0.1f;
        AttackRadius = 2;
        AttackAmount = 12;
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

    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 2, ExtraEffect, SwordLightRadius);//刀光伤害为2
        bullet.Shoot(direction, 10);
    }
}

public class Excalibur : Weapon
{
    public Excalibur()
    {
        AttackSpeed = 0.05f;
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

    //SwordLightAttack
    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //刀光
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 2, ExtraEffect, SwordLightRadius);//刀光伤害为2
        bullet.Shoot(direction, 10);
    }

}

public class Faith : Weapon
{
    public Faith()
    {
        AttackSpeed = 0.05f;
        AttackRadius = 2;
        AttackAmount = 2;
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

    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 1, ExtraEffect, SwordLightRadius);//刀光伤害为1
        bullet.Shoot(direction, 10);
    }

}

public class GilgameshEa : Weapon
{
    public GilgameshEa()
    {
        AttackSpeed = 0.1f;
        AttackRadius = 2;
        AttackAmount = 14;
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

    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 2, ExtraEffect, SwordLightRadius);//刀光伤害为2
        bullet.Shoot(direction, 10);
    }

}

public class MasterSword : Weapon
{
    public MasterSword()
    {
        AttackSpeed = 0.05f;
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

    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 2, ExtraEffect, SwordLightRadius);//刀光伤害为2
        bullet.Shoot(direction, 10);
    }

}

public class VirtuousTreaty : Weapon
{
    public VirtuousTreaty()
    {
        AttackSpeed = 0.05f;
        AttackRadius = 2;
        AttackAmount = 5;
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

    public override void SwordLightAttack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 1f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * 1, ExtraEffect, SwordLightRadius);//刀光伤害为1
        bullet.Shoot(direction, 10);
    }

}

public class xianyu : Weapon
{
    public xianyu()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 16;
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
    //咸鱼武器特点：没有刀光，但是近战伤害高
}

public class xianyu_2 : Weapon
{
    public xianyu_2()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 12;
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
    //咸鱼武器特点：没有刀光，但是近战伤害高
}

public class xianyu_3 : Weapon
{
    public xianyu_3()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 8;
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
    //咸鱼武器特点：没有刀光，但是近战伤害高
}

public class xianyu_4 : Weapon
{
    public xianyu_4()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 4;
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
    //咸鱼武器特点：没有刀光，但是近战伤害高
}

public class qingqing : Weapon
{
    public qingqing()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 16;
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
    //清青鸡肉卷的特点与咸鱼相同
}

public class qingqing_2 : Weapon
{
    public qingqing_2()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 12;
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
    //清青鸡肉卷的特点与咸鱼相同
}

public class qingqing_3 : Weapon
{
    public qingqing_3()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 8;
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
    //清青鸡肉卷的特点与咸鱼相同
}

public class qingqing_4 : Weapon
{
    public qingqing_4()
    {
        AttackSpeed = 0.2f;
        AttackRadius = 2;
        AttackAmount = 4;
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
    //清青鸡肉卷的特点与咸鱼相同
}

public class Gun : Weapon
{
    public Gun()
    {
        handleOffset = new Vector3 (0.5f, 0.3f, 0f);
        AttackSpeed = 0.2f;
        AttackRadius = 20;
        AttackAmount = 5;
        AttackAngle = 0;
        IsStackable = false;
        Amount = 1;
    }

    public override void Attack(CharacterAgent user, Vector3 direction)
    {
        //Debug.Log(user.WeaponPrefab.transform.position);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.WeaponPrefab.transform.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 20);
    }

}

public class EnergyGun : Weapon
{
    public EnergyGun()
    {
        AttackSpeed = 0.4f;
        AttackRadius = 20;
        AttackAmount = 10;
        AttackAngle = 0;
        IsStackable = false;
        AttackAmount = 8;
        Amount = 1;
    }

    public override void Attack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
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
        AttackSpeed = 0.4f;
        AttackRadius = 20;
        AttackAmount = 15;
        AttackAngle = 0;
        IsStackable = false;
        AttackAmount = 8;
        Amount = 1;
    }

    public override void Attack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
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
        AttackSpeed = 0.05f;
        AttackRadius = 20;
        AttackAmount = 5;
        AttackAngle = 0;
        IsStackable = false;
        AttackAmount = 8;
        Amount = 1;
    }

    public override void Attack(CharacterAgent user, Vector3 direction)
    {
        //Vector3 offset = new Vector3(1, 1, 0);
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + offset + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.SetBullet(user, user.ActualCharacter.AttackAmount * AttackAmount, ExtraEffect);
        bullet.Shoot(direction, 30);
    }

}

public class Puella : Weapon
{
    public Puella()
    {
        handleOffset = new Vector3(0.9f, 0.9f, 0f);
        AttackSpeed = 0.2f;
        AttackRadius = 20;
        AttackAmount = 15;
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
        bullet.Shoot(direction, 20);
    }

}

public class RathBuster : Weapon
{
    public RathBuster()
    {
        handleOffset = new Vector3(0.9f, 0.9f, 0f);
        AttackSpeed = 0.2f;
        AttackRadius = 20;
        AttackAmount =5;
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
        bullet.Shoot(direction, 20);
    }

}

public class RathGunlance : Weapon
{
    public RathGunlance()
    {
        handleOffset = new Vector3(0.9f, 0.9f, 0f);
        AttackSpeed = 0.05f;
        AttackRadius = 20;
        AttackAmount = 2;
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