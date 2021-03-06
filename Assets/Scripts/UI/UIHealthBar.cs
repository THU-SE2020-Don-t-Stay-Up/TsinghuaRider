﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    private Image mask;
    float originalSize;

    // 为了测试改成public，之后要改回private
    public void Awake()
    {
        mask = GameObject.Find("HealthMask").GetComponent<Image>();
        instance = this;

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
