using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class UIText : MonoBehaviour
{
    public Text Titles;
    private string[] text;
    int lineCount = 0;

    private void Start()
    {
        StartCoroutine(Display());
    }

    IEnumerator Display()
    {
        text = Regex.Split(Resources.Load<TextAsset>("Subtitles/Welcoming").text, Environment.NewLine, RegexOptions.IgnoreCase);
        lineCount = text.Length;
        Debug.Log(lineCount);

        for (int i = 0; i < lineCount; i++)
        {
            string tempText = text[i];
            Debug.Log(tempText);
            if (string.IsNullOrEmpty(tempText))
                continue;
            Titles.text = tempText.Split('$')[0];

            float tempTime;
            if (float.TryParse(tempText.Split('$')[1], out tempTime))
            {
                yield return new WaitForSeconds(tempTime);
            }
        }
    }



}
