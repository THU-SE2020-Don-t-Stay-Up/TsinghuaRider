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
    public int AttackAmount { get; set; }
    public float AttackRadius { get; set; }
    public int AttackAngle { get; set; }
    /// <summary>
    /// 实体各种状态
    /// </summary>
    public State State { get; set; } = new State();
    /// <summary>
    /// 无敌时间
    /// </summary>
    public float TimeInvincible { get; set; }
    /// <summary>
    /// 技能列表，存储实体可施放的技能
    /// </summary>
    [JsonConverter(typeof(SkillsJsonConverter))]
    public List<Skill> Skills { get; set; }
}

class SkillsJsonConverter : JsonConverter<List<Skill>>
{
    public override List<Skill> ReadJson(JsonReader reader, Type objectType, List<Skill> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (existingValue == null) existingValue = new List<Skill>();
        JArray arr = JArray.Load(reader);
        foreach (var s in arr)
        {
            existingValue.Add((Skill)Activator.CreateInstance(typeof(Skill).Assembly.GetType(s.ToString() + "Skill")));
        }
        return existingValue;
    }

    public override void WriteJson(JsonWriter writer, List<Skill> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        if (value != null)
        {
            foreach (var s in value)
            {
                string name = s.GetType().Name;
                writer.WriteValue(name.Substring(0, name.Length - 5));
            }
        }
        writer.WriteEndArray();
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

    /// <summary>
    /// 实际移动速度，受减速效果影响
    /// </summary>
    public float MoveSpeed;

    public Animator animator;
    public AudioSource audioSource;
    public AudioClip getHitClip;
    public AudioClip attackClip;
    public AudioClip getHealingClip;

    public Rigidbody2D rigidbody2d;
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (living.State.HasStatus(Status.Invincible))
            {
                Debug.Log($"{living.Name} Invincible");
                return;
            }

            else
            {
                living.CurrentHealth = Mathf.Clamp(living.CurrentHealth + amount, 0, living.MaxHealth);
                //animator.SetTrigger("Hit");
                //audioSource.PlayOneShot(getHitClip);
                //living.State.AddStatus(Status.Invincible, living.TimeInvincible);
                if (IsDead())
                {
                    //死亡动画
                    Destroy();
                }
            }
        }
        else
        {
            animator.SetTrigger("Heal");
            audioSource.PlayOneShot(getHealingClip);

            living.CurrentHealth = Mathf.Clamp(living.CurrentHealth + amount, 0, living.MaxHealth);
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
                living.State.RemoveStatus(status);
            }
        }
    }

    public bool IsDead()
    {
        if (living.CurrentHealth <= 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 血量为零时销毁gameObject
    /// </summary>
    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public List<GameObject> GetAttackRangeObjects(Vector3 attackPosition, string mask)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, living.AttackRadius, LayerMask.GetMask(mask));
        foreach (var collider in colliders)
        {
            Vector3 Direction = (attackPosition - transform.position).normalized;
            Vector3 v = collider.gameObject.transform.position - transform.position;
            float angle = Vector3.Angle(v, Direction);
            if (angle < living.AttackAngle)
            {
                gameObjects.Add(collider.gameObject);
            }
        }
        return gameObjects;
    }
}
