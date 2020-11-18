using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例化游戏中所有item，主要是给itemPrefab提供不同的图标以供选择
/// 脚本使用方法：在scene中创建一个gameobject，把这个脚本拖上去，在inspector中把物品的图片填进去。这样把物品和它的图片绑定。
/// </summary>
public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    public Transform ItemPrefab;
    public Sprite swordSprite;
    public Sprite healthPotionSprite;
    public Sprite strengthPotionSprite;
    public Sprite coinSprite;
    public Sprite medkitSprite;
 
}
