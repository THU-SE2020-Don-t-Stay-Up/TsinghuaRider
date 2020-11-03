using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
//using System;

public class MonsterAgent : MonoBehaviour
{
    Monster monster;
    public int monsterIndex;
    GameObject targetObject;
    float deltaTime = 0;
    //public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        monster = Global.monsters[monsterIndex];
        print(monster.Name);
        targetObject = GameObject.Find("targetObject");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = targetObject.transform.position;
        if (MoveToTarget(targetPosition))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = targetObject.transform.position;
        if (MoveToTarget(targetPosition))
        {
            Attack();
        }
    }

    bool MoveToTarget(Vector3 targetPosition)
    {
        Vector3 nowPosition = transform.position;
        Vector3 distance = targetPosition - nowPosition;
        Vector3 direction = Vector3.Normalize(distance);
        if (Vector3.Magnitude(distance) > monster.AttackRadius)
        {
            transform.Translate(monster.MoveSpeed * direction * Time.deltaTime);
            return false;
        }
        else 
        {
            return true;
        }
    }

    void Attack()
    {
        deltaTime += Time.deltaTime;
        if (monster.AttackSpeed - deltaTime < 0.01)
        {
            monster.Skills[0].Perform(monster.AttackAmount, targetObject);
            print("attack target");
            deltaTime = 0;
        }
    }
}