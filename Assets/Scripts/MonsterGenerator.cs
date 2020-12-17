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
    /// 从最多多少个生成点刷怪
    /// </summary>
    public int splitGeneratePoints;
    /// <summary>
    /// 生成第一波的延迟
    /// </summary>
    public float delay;
    /// <summary>
    /// 参考难度
    /// </summary>
    public float difficulty;
    /// <summary>
    /// 超时时间，存活到该时间过关
    /// </summary>
    public float endTime;

    /// <summary>
    /// 经过的总时间
    /// </summary>
    float elapsedTime;
    /// <summary>
    /// 计时生成怪的间隔
    /// </summary>
    float timer;
    bool generating = false;
    UITime ui;

    public void Generate()
    {
        timer = delay;
        generating = true;

        elapsedTime = 0;
        ui = FindObjectOfType<UITime>();
    }

    // Update is called once per frame
    void Update()
    {
        if (generating)
        {
            elapsedTime += Time.deltaTime;
            if (ui != null)
            {
                ui.SetRoomTime(Mathf.Max(endTime - elapsedTime, 0));
            }

            if (elapsedTime > endTime)
            {
                GameObject[] remainingMonsters = GameObject.FindGameObjectsWithTag("Monster");
                foreach (var monster in remainingMonsters)
                {
                    Destroy(monster);
                }

                Clear();
            }

            if (wave > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0 || NoMonster())
                {
                    for (int i = 0; i < splitGeneratePoints; i++)
                    {
                        int type = Random.Range(0, monsterGroups.Length);
                        int point = Random.Range(0, generatePoints.Length);

                        MonsterGroup monsterGroup = Instantiate(monsterGroups[type], generatePoints[point].position, Quaternion.identity).GetComponent<MonsterGroup>();
                        monsterGroup.Generate(difficulty);
                    }
                    wave -= 1;
                    timer = interval;
                }
            }
            else
            {
                // 所有怪已生成完毕且没有怪了
                if (NoMonster())
                {
                    Clear();
                }
            }
        }
    }

    /// <summary>
    /// 判断当前没有存活怪物和正在生成的怪物
    /// </summary>
    /// <returns></returns>
    private bool NoMonster()
    {
        return (null == GameObject.FindGameObjectWithTag("Monster") && null == GameObject.FindObjectOfType<MonsterGroup>());
    }

    private void Clear()
    {
        Room room = GetComponentInParent<Room>();
        if (room != null)
        {
            room.Clear();
        }
        generating = false;
    }
}
