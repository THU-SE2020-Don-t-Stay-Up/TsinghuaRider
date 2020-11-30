using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag(tag);
        Debug.Log("Follow: " + player);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
            transform.position = player.transform.position + Vector3.forward * -10;
    }
}
