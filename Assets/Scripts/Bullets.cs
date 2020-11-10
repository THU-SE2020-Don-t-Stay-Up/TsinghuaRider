using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    public float MaxRange = 100.0f;
    public float Velocity;
    public float AttackAmount { get; set; }
    public Vector3 Direction { get; set; }

    void Awake(){
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (transform.position.magnitude > MaxRange)
        {
            Destroy(gameObject);
        }
    }

    public void Fire(Vector3 dir3, float force){
        Vector2 dir2 = new Vector2(0,0);
        dir2.Set(dir3.x, dir3.y);
        rigidbody2d.AddForce(dir2 * force);
    }

    public void Move()
    {
        transform.Translate(Direction * Velocity * Time.deltaTime);
    }
}
