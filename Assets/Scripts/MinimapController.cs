using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    GameObject player;
    CharacterLoader characterLoader;

    // Start is called before the first frame update
    void Start()
    {
        characterLoader = FindObjectOfType<CharacterLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + Vector3.forward * -10;
        }
        else
        {
            player = characterLoader.player;
        }
    }
}
