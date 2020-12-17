using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterReset : MonoBehaviour
{
    CharacterLoader characterLoder;
    AcrossSceneController acrossSceneController;

    private void Awake()
    {
        characterLoder = GameObject.Find("CharacterLoader").GetComponent<CharacterLoader>();
        acrossSceneController = GameObject.Find("Across Scene Controller").GetComponent<AcrossSceneController>();
        acrossSceneController.ui.SetActive(true);
        characterLoder.player.GetComponent<CharacterAgent>().SetPortrait(acrossSceneController);
        characterLoder.player.GetComponent<CharacterAgent>().Initialize();


    }

}
