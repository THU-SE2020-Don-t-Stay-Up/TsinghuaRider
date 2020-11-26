using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 游戏实体基类，存储实体基本信息
/// </summary>
public class LivingBase
{
    public string Name { get; set; }
    public float MoveSpeed { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int Defense { get; set; }
    /// <summary>
    /// 韧性，减少控制时长
    /// </summary>
    public float Tenacity { get; }
    public float AttackSpeed { get; set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public int AttackAmount { get; set; }
    public float AttackRadius { get; set; }
    public int AttackAngle { get; set; }

    public bool isDead { get; set; } = false;
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
    public AudioClip GetHitClip { get; set; }
    public AudioClip AttackClip { get; set; }
    public AudioClip GetHealingClip { get; set; }

    public Rigidbody2D rigidbody2d { get; set; }

    public void ChangeHealth(float amount)
    {
        if (amount < 0)
        {
            if (living.State.HasStatus(new InvincibleState()))
            {
                Debug.Log($"{living.Name} Invincible");
                return;
            }

            else
            {
                living.CurrentHealth = (int)Mathf.Clamp(living.CurrentHealth + amount, 0, living.MaxHealth);
                Debug.Log($"{living.Name} now health is {living.CurrentHealth}");
                //animator.SetTrigger("Hit");
                //audioSource.PlayOneShot(getHitClip);
                living.State.AddStatus(new InvincibleState(), living.TimeInvincible);
                //print($"{living.Name}获得无敌{living.TimeInvincible}");
                if (!living.isDead && IsDead())
                {
                    //死亡动画
                    Destroy();
                }
            }
        }
        else
        {
            //animator.SetTrigger("Heal");
            //audioSource.PlayOneShot(getHealingClip);

            living.CurrentHealth = (int)Mathf.Clamp(living.CurrentHealth + amount, 0, living.MaxHealth);
        }
        // UI change
    }

    /// <summary>
    /// 检查状态，若持续时间为零则移除该状态，每帧调用
    /// </summary>
    public void CheckState()
    {
        foreach (var status in living.State.StateDuration.Keys.ToArray())
        {
            living.State.StateDuration[status] -= Time.deltaTime;
            if (living.State.StateDuration[status] <= 0)
            {
                status.Resume(this);
                living.State.RemoveStatus(status);
                print($"移除{living.Name}的{status}");
            }
            else
            {
                status.Effect(this);
            }
        }
    }

    public bool IsDead()
    {
        if (living.CurrentHealth <= 0)
        {
            living.isDead = true;
            return true;
        }
        else
            return false;
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
}
