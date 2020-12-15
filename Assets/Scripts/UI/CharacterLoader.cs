using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    GameObject mahou;
    GameObject robot;

    private void Awake()
    {
        robot = GameObject.Find("RobotPrefab");
        mahou = GameObject.Find("MahouPrefab");
        robot.SetActive(false);
        mahou.SetActive(false);
        if (UISelectCharacter.characterIndex ==0)
        {
            mahou.SetActive(true);
        }
        else
        {
            robot.SetActive(true);
        }
    }

    private void Update()
    {
        if (UISelectCharacter.characterIndex == 0)
        {
            this.transform.position = mahou.transform.position;
        }
        else
        {
            this.transform.position = robot.transform.position;
        }
    }
}
