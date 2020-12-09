using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup : MonoBehaviour
{
    /// <summary>
    /// 可生成的怪物prefab
    /// </summary>
    public GameObject[] monsterObjects;
    /// <summary>
    /// 参考生成概率
    /// </summary>
    public float[] probability;
    /// <summary>
    /// 参考难度（会影响生成怪物的数量）
    /// </summary>

    public void Generate(float difficulty)
    {
        //Debug.Log("generate monster group");
        float[] monsterDifficulty = new float[monsterObjects.Length];
        float usedDifficulty = 0.0f;
        float totalProbability = 0.0f;

        for(int i = 0; i < monsterObjects.Length; i++)
        {
            Monster monster = Global.monsters[monsterObjects[i].GetComponent<MonsterAgent>().monsterIndex];
            monsterDifficulty[i] = monster.Difficulty;

            totalProbability += probability[i];
        }
        
        while(usedDifficulty < difficulty)
        {
            float tmp = Random.Range(0, totalProbability);
            float current = 0.0f;
            for (int i = 0; i < monsterObjects.Length; i++)
            {
                current += probability[i];
                if (tmp < current)
                {
                    Instantiate(monsterObjects[i], transform.position, Quaternion.identity);
                    usedDifficulty += monsterDifficulty[i];
                    break;
                }
            }
        }

        Destroy(gameObject);
    }
}
