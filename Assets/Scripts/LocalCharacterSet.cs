using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCharacterSet : MonoBehaviour
{
    GameObject mahou;
    private void Awake()
    {
        UISelectCharacter.characterIndex = 0;
        mahou = GameObject.Find("MahouPrefab");
        CharacterAgent mahouAgent = mahou.GetComponent<CharacterAgent>();
        mahouAgent.enabled = true;
        mahouAgent.TestAwake();
    }
}
