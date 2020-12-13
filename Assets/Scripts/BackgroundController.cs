using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject mainCamera;

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        else
        {
            transform.position = mainCamera.transform.position * 0.6f;
        }
    }
}
