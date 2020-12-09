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
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

}
