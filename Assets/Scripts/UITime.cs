using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITime : MonoBehaviour
{
    TextMeshProUGUI roomTimeText;
    TextMeshProUGUI totalTimeText;

    // Start is called before the first frame update
    void Start()
    {
        roomTimeText = transform.Find("Room Time").GetComponent<TextMeshProUGUI>();
        totalTimeText = transform.Find("Total Time").GetComponent<TextMeshProUGUI>();
    }

    public void SetRoomTime(float time)
    {
        int minutes = (int)(time / 60);
        float seconds = (time % 60);
        string text = minutes.ToString() + ":" + seconds.ToString("00.0");
        roomTimeText.SetText(text);
    }

    public void SetTotalTime(float time)
    {
        int hours = (int)(time / 3600);
        int minutes = (int)((time % 3600) / 60);
        int seconds = (int)(time % 60);
        string text = hours.ToString() + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        totalTimeText.SetText(text);
    }
}
