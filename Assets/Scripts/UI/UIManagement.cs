using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagement : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("StartScene");
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
