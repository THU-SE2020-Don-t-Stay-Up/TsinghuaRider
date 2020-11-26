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

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        aimTransform = transform;
    }

    public void SetBullet(CharacterAgent user, Weapon weapon)
    {
        StartPoint = user.transform.position;
        Damage = user.ActualCharacter.AttackAmount * weapon.AttackAmount;
        UserTag = user.tag;
    }
    private void HandleAiming(Vector3 shootDir)
    {
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
        LivingBaseAgent living = gameObject.GetComponent<LivingBaseAgent>();
        if (living != null)
        {
            if (!gameObject.CompareTag(UserTag))
            {
                living.ChangeHealth(-Damage);
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
