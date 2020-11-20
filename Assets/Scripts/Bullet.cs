using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IInteract
{
    private Rigidbody2D rigidbody2d;
    private Transform aimTransform;

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
            living.ChangeHealth(-10);
        }
        Destroy(this.gameObject);

    }

    private void Update()
    {
        if (transform.position.magnitude > 50f)
        {
            Destroy(gameObject);
        }
    }

}
