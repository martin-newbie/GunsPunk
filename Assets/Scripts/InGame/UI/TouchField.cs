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
        if (InGameManager.Instance.isGameActive)
        {
            if (!InGameManager.Instance.tutorialTrigger) InGameManager.Instance.tutorialTrigger = true; 

            startPos = eventData.position;
            isDrag = true;
        }
    }

    float dragDelay = 0.2f;
    float curDelay;

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            dragPos = eventData.position;
            float dist = Vector3.Distance(startPos, dragPos);

            if(dist >= 300f)
            {
                isDrag = false;
                JumpFunction();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            isDrag = false;
            JumpFunction();
        }
    }

    void JumpFunction()
    {
        if (startPos.y < dragPos.y)
        {
            InGameManager.Instance.CurPlayer.GoUp();
        }
        else
        {
            InGameManager.Instance.CurPlayer.GoDown();
        }

        startPos = Vector2.zero;
        dragPos = Vector2.zero;
    }
}
