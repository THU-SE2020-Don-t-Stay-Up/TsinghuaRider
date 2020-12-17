using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPause : MonoBehaviour
{
    Transform pauseText;

    // Start is called before the first frame update
    void Start()
    {
        pauseText = transform.Find("PauseText");
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.gamePaused)
        {
            pauseText.gameObject.SetActive(true);
        }
        else
        {
            pauseText.gameObject.SetActive(false);
        }
    }
}
