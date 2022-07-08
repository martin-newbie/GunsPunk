using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchField : MonoBehaviour
{

    public RectTransform canvasRT;
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(canvasRT.sizeDelta.x / 2, 0f);
    }

    void Update()
    {
        
    }
}
