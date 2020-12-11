using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IInteract
{
    private Rigidbody2D rigidbody2d;
    private Transform aimTransform;
    public Vector3 StartPoint { get; set; }
    /// <summary>
    /// 子弹飞行的最大距离
    /// </summary>
    public float AttackRadius { get; set; }
    public float Damage { get; set; }

    public string UserTag { get; set; }

    public Action<LivingBaseAgent> ExtraEffect { get; set; }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        aimTransform = transform;
    }

    public void SetBullet(LivingBaseAgent user, float attackAmount, Action<LivingBaseAgent> extraEffect)
    {
        StartPoint = user.transform.position;
        UserTag = user.tag;
        Damage = attackAmount;
        ExtraEffect = extraEffect;
    }

    private void HandleAiming(Vector3 shootDir)
    {
        // 这个Awake()也是为了测试加上去的
        Awake();
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void Shoot(Vector3 shootDir, float speed)
    {
        HandleAiming(shootDir);
        rigidbody2d.AddForce(shootDir * speed, ForceMode2D.Impulse);
    }

    public void InteractWith(GameObject gameObject)
    {
        LivingBaseAgent agent = gameObject.GetComponent<LivingBaseAgent>();
        if (agent != null)
        {
            if (!gameObject.CompareTag(UserTag))
            {
                agent.ChangeHealth(-Damage);
                ExtraEffect?.Invoke(agent);
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        if ((transform.position - StartPoint).magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

}
