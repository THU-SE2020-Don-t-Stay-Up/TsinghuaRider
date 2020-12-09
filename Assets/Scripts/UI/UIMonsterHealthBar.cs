using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMonsterHealthBar : MonoBehaviour
{
    private Image mask;
    float originalSize;

    public void Awake()
    {
        mask = transform.Find("MonsterHealthMask").GetComponent<Image>();
    }

    private void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        // 为了Edit Mode测试，加入Awake()，之后需要删除
        Awake();
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

}
