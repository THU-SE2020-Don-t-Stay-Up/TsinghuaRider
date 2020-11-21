using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.0f;

    Rigidbody2D rigidbody;
    Vector2 move;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate() {
        Vector2 position = rigidbody.position;
        position.x += moveSpeed * move.x * Time.deltaTime;
        position.y += moveSpeed * move.y * Time.deltaTime;
        
        rigidbody.MovePosition(position);
    }
}
