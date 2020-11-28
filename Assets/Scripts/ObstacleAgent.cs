using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAgent : MonoBehaviour
{
    private bool IsTriggered;
    private float PrepareTime { get; set; }
    private float LastTime { get; set; }
    private float deltaTime;
    private PolygonCollider2D Collider2D { get; set; }
    private SpriteRenderer sprite;
    private Color color;
    // Start is called before the first frame update
    void Awake()
    {
        PrepareTime = 1.0f;
        LastTime = 3.0f;
        IsTriggered = false;
        sprite = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<PolygonCollider2D>();
        Collider2D.enabled = false;
        deltaTime = 0;
        color = sprite.color;
        color.a = 0;
        sprite.color = color;
        //GetComponent<BoxCollider2D>().
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTriggered)
        {
            color.a = deltaTime / PrepareTime;
            sprite.color = color;
            if (deltaTime > PrepareTime)
            {
                IsTriggered = true;
                Collider2D.enabled = true;
                deltaTime = 0;
            }
        }
        else
        {
            if (deltaTime > LastTime)
            {
                GameObject.Destroy(gameObject);
            }
        }
        deltaTime += Time.deltaTime;
    }


}
