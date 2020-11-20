using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject[] monsterObject;
    public int[] monsterNum;
    // Start is called before the first frame update
    void Start()
    {
        
        for (int index = 0; index < monsterNum.Length; index++)
        {
            for (int i = 0; i < monsterNum[index]; i++)
            {
                Vector3 position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));
                GameObject.Instantiate(monsterObject[index], position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
