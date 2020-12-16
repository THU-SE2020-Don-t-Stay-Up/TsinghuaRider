using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    GameObject mahou;
    GameObject robot;
    public GameObject player { get; set; }

    private void Awake()
    {
        robot = GameObject.Find("RobotPrefab");
        mahou = GameObject.Find("MahouPrefab");
    }

    public void LoadCharacter()
    {
        if (UISelectCharacter.characterIndex == 0)
        {
            player = mahou;
        }
        else
        {
            player = robot;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }
}
