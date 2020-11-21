﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

interface IWeapon
{
    void Attack(GameObject user, Vector3 direction);
}
/// <summary>
/// 远程武器类
/// </summary> 
public class MissleWeapon : Item, IWeapon
{
    /// <summary>
    /// 子弹prefab
    /// </summary>
    public GameObject bulletPrefab;

    public void Attack(GameObject user, Vector3 direction)
    {
        GameObject projectileObject = GameObject.Instantiate(bulletPrefab, user.transform.position + direction * 0.5f, Quaternion.identity);
        Bullet bullet = projectileObject.GetComponent<Bullet>();
        bullet.startPoint = user.transform.position;
        bullet.Damage = user.GetComponent<LivingBaseAgent>().living.AttackAmount;
        bullet.userTag = user.tag;
        bullet.Shoot(direction, 10);
    }

    public override void Use(CharacterAgent character)
    {
        throw new System.NotImplementedException();
    }


}

public class MeleeWeapon : Item, IWeapon
{
    public void Attack(GameObject user, Vector3 Direction)
    {
        string layerMask;
        LivingBaseAgent agent = user.GetComponent<LivingBaseAgent>();
        // 判断进行攻击的是角色还是怪物
        if (agent is CharacterAgent)
        {
            layerMask = "Monster";
        }
        else
        {
            layerMask = "Player";
        }
        IEnumerable<GameObject> targetObjects = agent.GetAttackRangeObjects(user.transform.position, agent.living.AttackDirection, layerMask);
        foreach (var targetObject in targetObjects)
        {
            LivingBaseAgent targetAgent = targetObject.GetComponent<LivingBaseAgent>();
            targetAgent.ChangeHealth(-agent.living.AttackAmount);
        }
    }

    public override void Use(CharacterAgent character)
    {
        throw new System.NotImplementedException();
    }


}