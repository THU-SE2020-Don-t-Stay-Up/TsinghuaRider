using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个代码存放所有游戏运行初始化的逻辑
/// </summary>
public class Initialization : MonoBehaviour
{
    public float initDifficulty;
    public float difficultyStep;

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
        GameObject minimapCamera = GameObject.Find("Minimap Camera");
        GameObject acrossSceneController = GameObject.Find("Across Scene Controller");
        GameObject characterLoder = GameObject.Find("CharacterLoader");
        GameObject eventSystem = GameObject.Find("EventSystem");

        try
        {
            DontDestroyOnLoad(mainCamera);
            DontDestroyOnLoad(vCamera);
            DontDestroyOnLoad(ui);
            DontDestroyOnLoad(minimapCamera);
            DontDestroyOnLoad(acrossSceneController);
            DontDestroyOnLoad(characterLoder);
            DontDestroyOnLoad(eventSystem);

            acrossSceneController.GetComponent<AcrossSceneController>().ui = ui;
            ui.SetActive(false);
        }
        catch (NullReferenceException)
        {
        }

        Global.difficulty = initDifficulty;
        Global.difficultyStep = difficultyStep;

        Global.totalTime = 0;
        Global.gamePaused = false;

        Destroy(gameObject);

    }

    public void TestAwake()
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
        GameObject characterLoder = GameObject.Find("CharacterLoader");
        GameObject eventSystem = GameObject.Find("EventSystem");
    }
}
