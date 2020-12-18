using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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
        string[] text = Regex.Split(Resources.Load<TextAsset>("Welcoming").text, "\r\n", RegexOptions.IgnoreCase);
        lineCount = text.Length;

        for (int i = 0; i < lineCount; i++)
        {
            string tempText = text[i];
            Titles.text = tempText.Split('$')[0];

            float tempTime;
            if (float.TryParse(tempText.Split('$')[1], out tempTime))
            {
                yield return new WaitForSeconds(tempTime);
            }
        }
    }



}
