using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public float teleportTime = 1.5f;
    public string teleportTag;
    public Transform teleportDestination;
    public AudioSource AudioSource { get; set; }
    public AudioClip teleportAudioClip;
    float timer;
    bool isTeleporting = false;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(teleportTag);
        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = teleportAudioClip;
        timer = teleportTime;
    }

    private void Update()
    {
        if (isTeleporting)
        {
            AudioSource.Play();
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                player.transform.position = teleportDestination.position;
                AudioSource.Stop();
            }
        }
        else
        {
            AudioSource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player)
        {
            Debug.Log("In Scene Teleporting " + player);
            isTeleporting = true;
            AudioSource.PlayOneShot(teleportAudioClip);
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
