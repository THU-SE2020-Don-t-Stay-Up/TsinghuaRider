using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectCharacter : MonoBehaviour
{
    public static int characterIndex = 0;
    GameObject robot;
    GameObject mahou;

    private void Awake()
    {
        robot = GameObject.Find("RobotPrefab");
        mahou = GameObject.Find("MahouPrefab");

        robot.GetComponent<BoxCollider2D>().enabled = false;
        robot.GetComponent<CharacterAgent>().enabled = false;
        robot.GetComponent<Animator>().enabled = false;

        mahou.GetComponent<BoxCollider2D>().enabled = false;
        mahou.GetComponent<CharacterAgent>().enabled = false;
        mahou.GetComponent<Animator>().enabled = false;


    }

    public void SelectRobot()
    {
        characterIndex = 1;

        mahou.GetComponent<CharacterAgent>().Stop();
        mahou.GetComponent<CharacterAgent>().enabled = false;

        robot.GetComponent<BoxCollider2D>().enabled = true;
        robot.GetComponent<CharacterAgent>().enabled = true;
        robot.GetComponent<Animator>().enabled = true;
        
        //mahou.GetComponent<Animator>().enabled = false;
    }

    public void SelectMahou()
    {
        characterIndex = 0;

        robot.GetComponent<CharacterAgent>().Stop();
        robot.GetComponent<CharacterAgent>().enabled = false;

        mahou.GetComponent<BoxCollider2D>().enabled = true;
        mahou.GetComponent<CharacterAgent>().enabled = true;
        mahou.GetComponent<Animator>().enabled = true;


       // robot.GetComponent<Animator>().enabled = false;
    }

}
