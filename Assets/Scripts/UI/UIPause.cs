using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPause : MonoBehaviour
{
    TextMeshProUGUI pauseText;

    // Start is called before the first frame update
    void Start()
    {
        pauseText = transform.Find("PauseText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.gamePaused)
        {
            pauseText.enabled = true;
        }
        else
        {
            pauseText.enabled = false;
        }
    }
}
