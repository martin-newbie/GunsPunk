using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeContainer : MonoBehaviour
{
    public Image Back;
    public Image Gauge;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetSprite(Sprite back, Sprite gauge)
    {
        Back.sprite = back;
        Gauge.sprite = gauge;
    }

    public void SetColor(Color back, Color gauge)
    {
        Back.color = back;
        Gauge.color = gauge;
    }

    public void FollowTarget(Vector3 offset, Transform target)
    {
        transform.position = target.position + offset;
    }

    public void SetGauge(float cur, float max)
    {
        Gauge.fillAmount = cur / max;
    }

}
