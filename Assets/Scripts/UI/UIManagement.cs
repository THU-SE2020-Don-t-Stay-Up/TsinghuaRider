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
        if (UISelectCharacter.characterIndex == -1)
            return;
        characterLoader = FindObjectOfType<CharacterLoader>();
        characterLoader.LoadCharacter();
        teleporter = Instantiate(sceneTeleporter, Vector3.zero, Quaternion.identity).GetComponent<SceneTeleporter>();
        teleporter.targetScene = "StartScene";
        teleporter.TeleportNow(characterLoader.player);
    }

    public static void ReturnMainPage()
    {       
        SceneManager.LoadScene("MainScene");
        SceneManager.UnloadScene(SceneManager.GetActiveScene());
    }

    public void SelectCharacter()
    {
        SceneManager.LoadScene("SelectCharacters");
        SceneManager.UnloadScene(SceneManager.GetActiveScene());
    }

    public void AboutGame()
    {
        SceneManager.LoadScene("AboutScene");
        SceneManager.UnloadScene(SceneManager.GetActiveScene());
    }

    public void GameHelp()
    {
        SceneManager.LoadScene("HelpScene");
        SceneManager.UnloadScene(SceneManager.GetActiveScene());
    }

    public void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = true;
        Application.Quit();
    }

}
