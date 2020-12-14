using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAgent : MonoBehaviour
{
    private bool IsTriggered;
    private bool IsFinished;
    private float PrepareTime { get; set; }
    public float LastTime;
    private float deltaTime;
    private Collider2D Collider2D { get; set; }
    private SpriteRenderer sprite;
    private Color color;
    // Start is called before the first frame update
    void Awake()
    {
        PrepareTime = 1.0f;
        IsTriggered = false;
        IsFinished = false;
        sprite = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<Collider2D>();
        Collider2D.enabled = false;
        deltaTime = 0;
        color = sprite.color;
        color.a = 0;
        sprite.color = color;
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
            if (!IsFinished)
            {
                if (deltaTime > LastTime)
                {
                    IsFinished = true;                   
                    deltaTime = 0;
                }
            }
            else
            {
                color.a = 1 - deltaTime / PrepareTime;
                sprite.color = color;
                if (deltaTime > PrepareTime)
                {
                    GameObject.Destroy(gameObject);
                    deltaTime = 0;
                }
            }
        }
        deltaTime += Time.deltaTime;
    }

    public void OnTriggerStay2D(Collider2D other) 
    {
        LivingBaseAgent agent = null;
        try
        {
            agent = other.gameObject.GetComponent<LivingBaseAgent>();
        }
        catch(Exception e)
        {
        }
        if (agent != null)
        {
            agent.actualLiving.State.AddStatus(new BleedState(5), 1);
            agent.actualLiving.State.AddStatus(new SlowState(0.5f), 1);
        }
    }
}
