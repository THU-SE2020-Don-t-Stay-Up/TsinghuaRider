using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void Awake()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        GameObject vCamera = GameObject.Find("CM vcam1");
        GameObject ui = GameObject.Find("UI");
        GameObject minimapCamera = GameObject.Find("Minimap Camera");
        GameObject acrossSceneController = GameObject.Find("Across Scene Controller");
        GameObject characterLoder = GameObject.Find("CharacterLoader");
        GameObject eventSystem = GameObject.Find("EventSystem");

        try
        {
            Destroy(mainCamera);
            Destroy(vCamera);
            Destroy(ui);
            Destroy(minimapCamera);
            Destroy(acrossSceneController);
            Destroy(characterLoder);
            Destroy(eventSystem);
        }
        catch (NullReferenceException)
        {
        }
    }


}
