using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("房间属性")]
    public float width = 10;
    public float height = 10;

    [Header("关联预制体")]
    public GameObject doorUp; /**< 保存门的prefab对象*/
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    public bool flagUp { set; private get; }
    public bool flagDown { set; private get; }
    public bool flagLeft { set; private get; } /**< 记录上下左右是否有房间，以此判断是否需要生成门*/
    public bool flagRight { set; private get; }
    
    public bool stageClear { set; get; }

    void Awake() {
        stageClear = false;      
    }

    void Start()
    {
        doorLeft.SetActive(flagLeft);
        doorRight.SetActive(flagRight);
        doorUp.SetActive(flagUp);
        doorDown.SetActive(flagDown);
    }

}
