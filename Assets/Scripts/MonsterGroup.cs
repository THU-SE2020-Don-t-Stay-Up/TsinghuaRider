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
    /// 生成延迟
    /// </summary>
    public float delay;
    public GameObject visualEffect;

    bool generating = false;
    float timer = 0;
    float difficulty;
    GameObject effect;
    Animator effectAnimator;

    public void Generate(float _difficulty)
    {
        difficulty = _difficulty;
        generating = true;
        timer = delay;

        // 生成特效
        effect = Instantiate(visualEffect, transform);
        effect.transform.localScale *= Mathf.Sqrt(difficulty);
        effectAnimator = effect.GetComponent<Animator>();
        effectAnimator.speed = 1.0f / (delay + 1e-4f);
    }

    private void Update()
    {
        if (generating)
        {
            timer -= Time.deltaTime;

            
            // 随机生成怪物，直到难度用完。生成后摧毁自身。
            if (timer < 0)
            {
                float[] monsterDifficulty = new float[monsterObjects.Length];
                float usedDifficulty = 0.0f;
                float totalProbability = 0.0f;

                for (int i = 0; i < monsterObjects.Length; i++)
                {
                    Monster monster = Global.monsters[monsterObjects[i].GetComponent<MonsterAgent>().monsterIndex];
                    monsterDifficulty[i] = monster.Difficulty;

                    totalProbability += probability[i];
                }

                while (usedDifficulty < difficulty)
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

    }

}
