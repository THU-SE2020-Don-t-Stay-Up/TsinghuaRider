using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AcrossSceneController : MonoBehaviour
{
    RoomGenerator roomGenerator;
    public float initDifficulty;
    public float deltaDifficulty;

    float difficulty;

    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        difficulty = initDifficulty;
    }

    void OnSceneUnloaded(Scene previous)
    {
        Debug.Log("Scene unloaded " + previous.name);
        roomGenerator = FindObjectOfType<RoomGenerator>();
        //Debug.Log(roomGenerator);
        roomGenerator.difficulty = difficulty;
        roomGenerator.Generate();
        difficulty += deltaDifficulty;

        SetupPlayer();
    }

    void SetupPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // 将玩家回到初始位置
            player.transform.position = transform.position;
            // 给玩家补满血
            player.GetComponent<LivingBaseAgent>().RestoreHealth();
        }
    }
}
