using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject doorLeft, doorRight, doorUp, doorDown; /**< 保存门的prefab对象*/
    
    public bool flagLeft, flagRight, flagUp, flagDown; /**< 记录上下左右是否有房间，以此判断是否需要生成门*/

    
    void Start()
    {
        doorLeft.SetActive(flagLeft);
        doorRight.SetActive(flagRight);
        doorUp.SetActive(flagUp);
        doorDown.SetActive(flagDown);
    }

    
    void Update()
    {
        
    }
}
