using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实例化游戏中所有item，配合Item类能返回item的图像，配合ItemWorld能够生成ItemWorld实例。
/// 脚本使用方法：在scene中创建一个gameobject，把这个脚本拖上去，在inspector中把物品的图片填进去。这样把物品和它的图片绑定。
/// </summary>
public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set;}

    private void Awake()
    {
        Instance = this;
    }
    public Transform ItemWorldPrefab;

    public Sprite swordSprite;
    public Sprite healthPotionSprite;
    public Sprite manaPotionSprite;
    public Sprite coinSprite;
    public Sprite medkitSprite;


}
