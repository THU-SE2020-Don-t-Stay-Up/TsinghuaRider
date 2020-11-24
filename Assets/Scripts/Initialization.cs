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
        Debug.Log("init");
        Global.LoadPrefabPaths();
        Global.monsters = Monster.LoadMonster();
        Global.characters = Character.LoadCharacter();
        Global.items = Item.LoadItem();

        GameObject mainCamera = GameObject.Find("Main Camera");
        GameObject vCamera = GameObject.Find("CM vcam1");
        GameObject ui = GameObject.Find("UI");
        //GameObject itemAssets = GameObject.Find("ItemAssets");
        DontDestroyOnLoad(mainCamera);
        DontDestroyOnLoad(vCamera);
        DontDestroyOnLoad(ui);
        //DontDestroyOnLoad(itemAssets);


        Destroy(gameObject);
    }
}
