using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public float teleportTime = 1.0f;
    public string teleportTag;
    public Transform teleportDestination;

    float timer;
    bool isTeleporting = false;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(teleportTag);

        timer = teleportTime;
    }

    private void Update()
    {
        if (isTeleporting)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                player.transform.position = teleportDestination.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player)
        {
            Debug.Log("In Scene Teleporting " + player);
            isTeleporting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isTeleporting = false;
            timer = teleportTime;
        }
    }
}
