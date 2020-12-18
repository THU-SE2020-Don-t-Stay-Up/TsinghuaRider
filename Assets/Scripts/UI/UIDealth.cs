using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDealth : MonoBehaviour
{
    public static UIDealth instance;
    Transform deathText;

    private void Start()
    {
        deathText = transform.Find("DeathText");
        instance = this;
        deathText.gameObject.SetActive(false);
    }

    public void DeathInfo()
    {
        deathText.gameObject.SetActive(true);
    }
    
}
