using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class GaugeGlitter : MonoBehaviour
{
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public float startFill = 0.98f;
    public float endFill = 0.05f;
    public float offset;
    public Direction Dir;
    public Image Gauge;
    public Image thisImg;
    RectTransform rect;

    private void Start()
    {
        Gauge = GetComponentsInParent<Image>()[1];
        thisImg = GetComponentsInParent<Image>()[0];
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Gauge.fillAmount < startFill && Gauge.fillAmount > endFill)
        {
            // animation start
            thisImg.enabled = true; // temp

            float pos_x = (Dir == Direction.Horizontal ? Gauge.rectTransform.sizeDelta.x : Gauge.rectTransform.sizeDelta.y) * Gauge.fillAmount;
            Vector2 pos = new Vector2();
            switch (Dir)
            {
                case Direction.Horizontal:
                    pos = new Vector2(pos_x - offset, 0);
                    break;
                case Direction.Vertical:
                    pos = new Vector2(0, pos_x - offset);
                    break;
            }

            rect.anchoredPosition = pos;
        }
        else
        {
            thisImg.enabled = false; // temp
            // animation stop
            // animation off
        }
    }
}
