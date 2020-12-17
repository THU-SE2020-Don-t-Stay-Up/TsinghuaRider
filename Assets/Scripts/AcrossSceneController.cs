using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AcrossSceneController : MonoBehaviour
{
    public GameObject ui = null;
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;       
    }

    void OnSceneUnloaded(Scene previous)
    {
        Debug.Log("Scene unloaded " + previous.name);
        RoomGenerator roomGenerator = FindObjectOfType<RoomGenerator>();
        if (roomGenerator != null)
        {
            roomGenerator.Generate();
        }
        SceneTeleporter[] teleporters = FindObjectsOfType<SceneTeleporter>();
        foreach (var teleporter in teleporters)
        {
            Debug.Log(teleporter);
            teleporter.FindTarget();
        }
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
