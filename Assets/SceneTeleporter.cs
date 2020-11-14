using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    public float teleportTime = 1.0f;
    public string teleportTag;
    public string targetScene;
    public GameObject mainCamera;
    public GameObject vCamera;

    float timer;
    bool isTeleporting = false;
    bool hasTeleported = false;
    GameObject player;

    void Start()
    {
        timer = teleportTime;
        player = GameObject.FindGameObjectWithTag(teleportTag);

        DontDestroyOnLoad(mainCamera);
        DontDestroyOnLoad(vCamera);
    }

    void Update()
    {
        if (isTeleporting)
        {
            // 可显示UI
            timer -= Time.deltaTime;
            if (timer < 0 && !hasTeleported)
            {
                StartCoroutine(TeleportAsync());
                hasTeleported = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Across Scene Teleporting " + player);
            isTeleporting = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isTeleporting = false;
            timer = teleportTime;
        }
    }

    IEnumerator TeleportAsync()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(targetScene));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

}
