using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public RectTransform canvasRT;
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(canvasRT.sizeDelta.x / 2, 0f);
    }

    Vector2 startPos;
    Vector2 dragPos;
    bool isDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            dragPos = eventData.position;

            if (dragPos.x < canvasRT.sizeDelta.x / 2)
            {
                isDrag = false;
                // jump function
                return;
            }


        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            isDrag = false;
            // jump function
        }
    }

    void JumpFunction()
    {

    }
}
