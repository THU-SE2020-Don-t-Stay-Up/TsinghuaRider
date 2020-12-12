using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    /// <summary>
    /// 可生成的怪物组
    /// </summary>
    public GameObject[] monsterGroups;
    /// <summary>
    /// 生成点，每个生成点每波生成一组怪物
    /// </summary>
    public Transform[] generatePoints;
    /// <summary>
    /// 生成波数
    /// </summary>
    public int wave;
    /// <summary>
    /// 每波时间间隔
    /// </summary>
    public float interval;
    /// <summary>
    /// 生成第一波的延迟
    /// </summary>
    public float delay;
    /// <summary>
    /// 总参考难度
    /// </summary>
    public float difficulty;

    float timer;
    bool generating = false;

    public void Generate()
    {
        timer = delay;
        generating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (generating)
        {
            if (wave > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    int type = Random.Range(0, monsterGroups.Length);
                    int point = Random.Range(0, generatePoints.Length);

                    MonsterGroup monsterGroup = Instantiate(monsterGroups[type], generatePoints[point].position, Quaternion.identity).GetComponent<MonsterGroup>();
                    monsterGroup.Generate(difficulty);

                    wave -= 1;
                    timer = interval;
                }
            }
            else
            {
                // 所有怪已生成完毕且没有怪了
                if (null == GameObject.FindGameObjectWithTag("Monster") && null == GameObject.FindObjectOfType<MonsterGroup>())
                {
                    Room room = GetComponentInParent<Room>();
                    if (room != null)
                    {
                        room.Clear();
                    }
                    generating = false;
                }
            }
        }
    }
}
