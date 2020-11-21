using UnityEngine;

public class Bullet : MonoBehaviour, IInteract
{
    private Rigidbody2D rigidbody2d;
    private Transform aimTransform;
    public Vector3 startPoint;
    public int Damage { get; set; }
    public string userTag;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        aimTransform = transform;
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
            if (!gameObject.CompareTag(userTag))
            {
                living.ChangeHealth(-Damage);
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        if ((transform.position - startPoint).magnitude > 50f)
        {
            Destroy(gameObject);
        }
    }

}
