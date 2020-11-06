using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public string teleportTag;
    public Transform teleportDestination;
    public bool inScene = true;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == teleportTag) {
            Debug.Log("Teleport");
            other.gameObject.transform.position = teleportDestination.position;
        }
    }
}
