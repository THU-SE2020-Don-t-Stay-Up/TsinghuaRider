using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagement : MonoBehaviour
{
    CharacterLoader characterLoader;
    public GameObject sceneTeleporter;
    SceneTeleporter teleporter;

    public void StartGame()
    {
        characterLoader = FindObjectOfType<CharacterLoader>();
        characterLoader.LoadCharacter();
        teleporter = Instantiate(sceneTeleporter, Vector3.zero, Quaternion.identity).GetComponent<SceneTeleporter>();
        teleporter.targetScene = "StartScene";
        teleporter.TeleportNow(characterLoader.player);
    }

    public void GameSetting() 
    {
        SceneManager.LoadScene("GameSetting");
    }

    public void ReturnMainPage()
    {
        SceneManager.LoadScene("MainScene");
    }

}
