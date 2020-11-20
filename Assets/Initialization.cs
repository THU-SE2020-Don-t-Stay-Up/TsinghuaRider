using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个代码存放所有游戏运行初始化的逻辑
/// </summary>
public class Initialization : MonoBehaviour
{

    private void Awake()
    {
        Global.monsters = Monster.LoadMonster();
        Global.characters = Character.LoadCharacter();

        Destroy(gameObject);
    }
}
