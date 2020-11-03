using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject monsterObject;
    public int monsterNum;
    // Start is called before the first frame update
    void Start()
    {
        Global.monsters = Monster.LoadMonster();
        for(int i = 0; i < monsterNum; i++)
        {
            Vector3 position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));
            GameObject.Instantiate(monsterObject, position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
