using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// 游戏实体基类，存储实体基本信息
/// </summary>
public class LivingBase:ICloneable
{
    public string Name { get; set; }
    public float MoveSpeed { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int Defense { get; set; }
    /// <summary>
    /// 韧性，减少控制时长
    /// </summary>
    public float Tenacity { get; set; }
    public float AttackSpeed { get; set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public float AttackAmount { get; set; }
    public float AttackRadius { get; set; }
    public int AttackAngle { get; set; }

    public int isDead = 0;
    /// <summary>
    /// 攻击方向
    /// </summary>
    public Vector3 AttackDirection { get; set; } = Vector3.zero;
    /// <summary>
    /// 实体各种状态
    /// </summary>
    public State State { get; set; } = new State();
    /// <summary>
    /// 无敌时间
    /// </summary>
    public float TimeInvincible { get; set; }

    public virtual object Clone()
    {
        LivingBase living = MemberwiseClone() as LivingBase;
        living.State = new State();
        return living;
    }

    public void Sync(LivingBase actualLiving)
    {
        actualLiving.MoveSpeed = MoveSpeed;
        actualLiving.MaxHealth = MaxHealth;
        actualLiving.Defense = Defense;
        actualLiving.Tenacity = Tenacity;
        actualLiving.AttackSpeed = AttackSpeed;
        actualLiving.AttackAmount = AttackAmount;
        actualLiving.AttackRadius = AttackRadius;
        //actualLiving.AttackAngle = AttackAngle;
        //actualLiving.TimeInvincible = TimeInvincible;
    }
}

/// <summary>
/// 游戏实体Agent基类，继承MonoBehavior
/// </summary>
public class LivingBaseAgent : MonoBehaviour
{
    /// <summary>
    /// 实体属性信息
    /// </summary>
    public LivingBase living;

    public LivingBase actualLiving;

    /// <summary>
    /// 实际移动速度，受减速效果影响
    /// </summary>
    //public float MoveSpeed { get; set; }
    public Animator Animator { get; set; }
    public AudioSource AudioSource { get; set; }
    public AudioClip MeleeAttackClip;
    public AudioClip MissleAttackClip;
    public AudioClip HealthPotionClip;
    public AudioClip MedkitClip;
    public AudioClip StrengthPotionClip;
    public Rigidbody2D rigidbody2d { get; set; }

    public Collider2D collider2d { get; set; }

    public virtual void ChangeHealth(float amount)
    {
        if (amount < 0)
        {
            if (actualLiving.State.HasStatus(new InvincibleState()))
            {
                Debug.Log($"{actualLiving.Name} Invincible");
                return;
            }

            else
            {
                actualLiving.CurrentHealth = (int)Mathf.Clamp(actualLiving.CurrentHealth + amount, 0, actualLiving.MaxHealth);
                actualLiving.State.AddStatus(new InvincibleState(), actualLiving.TimeInvincible);
                if (IsDead())
                {
                    if (Interlocked.Exchange(ref actualLiving.isDead, 1) == 0)
                    {
                        //死亡动画
                        Destroy();
                    }
                }
            }
        }
        else
        {
            actualLiving.CurrentHealth = (int)Mathf.Clamp(actualLiving.CurrentHealth + amount, 0, actualLiving.MaxHealth);
        }
    }
    public void RestoreHealth()
    {
        ChangeHealth(actualLiving.MaxHealth);
    }
    /// <summary>
    /// 为生物添加状态
    /// </summary>
    /// <param name="status">状态</param>
    /// <param name="duration">持续时间</param>
    public void AddStatus(StateBase status, float duration)
    {
        actualLiving.State.AddStatus(status, duration);
    }
    /// <summary>
    /// 检查状态，若持续时间为零则移除该状态，每帧调用
    /// </summary>
    public void CheckState()
    {
        //living.Sync(actualLiving);
        foreach (var status in actualLiving.State.StateDuration.Keys.ToArray())
        {
            actualLiving.State.StateDuration[status] -= Time.deltaTime;
            if (actualLiving.State.StateDuration[status] <= 0)
            {
                status.Resume(this);
                actualLiving.State.RemoveStatus(status);
                print($"移除{actualLiving.Name}的{status}");
            }
            else
            {
                status.Effect(this);
            }
        }
    }

    public bool IsDead()
    {
        return actualLiving.CurrentHealth <= 0;
    }

    /// <summary>
    /// 血量为零时销毁gameObject
    /// </summary>
    public virtual void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// 获取攻击方向内可攻击实体列表
    /// </summary>
    /// <param name="attackDirection">攻击方向</param>
    /// <param name="mask">layerMask名称</param>
    /// <returns>GameObject列表</returns>
    public IEnumerable<GameObject> GetAttackRangeObjects(Vector3 position, Vector3 attackDirection, float attackRadius, float attackAngle, params string[] mask)
    {
        return from collider in Physics2D.OverlapCircleAll(position, attackRadius, LayerMask.GetMask(mask))
               let targetDirection = collider.gameObject.transform.position - transform.position
               let angle = Vector3.Angle(targetDirection, attackDirection)
               where angle < attackAngle
               select collider.gameObject;
    }

    public Vector3 GetCentralPosition()
    {
        return collider2d.offset + rigidbody2d.position;
    }

    public virtual Vector3 GetAttackDirection() { return Vector3.zero; }
}
