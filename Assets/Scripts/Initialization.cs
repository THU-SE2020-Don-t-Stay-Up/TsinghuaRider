using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个代码存放所有游戏运行初始化的逻辑
/// </summary>
public class Initialization : MonoBehaviour
{

    public void Awake()
    {
        Debug.Log("init");
        Global.LoadPrefabPaths();
        Global.monsters = Monster.LoadMonster();
        Global.characters = Character.LoadCharacter();
        Global.items = Item.LoadItem();

        GameObject mainCamera = GameObject.Find("Main Camera");
        GameObject vCamera = GameObject.Find("CM vcam1");
        GameObject ui = GameObject.Find("UI");
        GameObject minimapCamera = GameObject.Find("Minimap Camera");
        GameObject acrossSceneController = GameObject.Find("Across Scene Controller");
        
        // 为了进行editor mode测试，把以下部分注释，之后需要还原！
        /*
        DontDestroyOnLoad(mainCamera);
        DontDestroyOnLoad(vCamera);
        DontDestroyOnLoad(ui);
        DontDestroyOnLoad(minimapCamera);
        DontDestroyOnLoad(acrossSceneController);
        

        Destroy(gameObject);
        */
    }
}
