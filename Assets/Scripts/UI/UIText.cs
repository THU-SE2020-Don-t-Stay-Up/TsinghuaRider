using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public Text Titles;
    StreamReader sr;
    int lineCount = 0;

    private void Start()
    {
        StartCoroutine(Display());
    }

    IEnumerator Display()
    {
        sr = new StreamReader(Application.dataPath + "/Welcoming.txt");
        
        StreamReader srLine = new StreamReader(Application.dataPath + "/Welcoming.txt");
        
        while(srLine.ReadLine() != null)
        {
            lineCount++;
        }

        srLine.Close();
        srLine.Dispose();
        for (int i = 0; i < lineCount; i++)
        {
            string tempText = sr.ReadLine();
            Titles.text = tempText.Split('$')[0];

            float tempTime;
            if (float.TryParse(tempText.Split('$')[1], out tempTime))
            {
                yield return new WaitForSeconds(tempTime);
            }
        }
        sr.Close();
        sr.Dispose();
    }



}
