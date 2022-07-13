using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireButton : MonoBehaviour
{
    Image ButtonImage;

    public Sprite PressedSprite;
    public Sprite DefaultSprite;

    bool pointerDown;

    void Start()
    {
        ButtonImage = GetComponent<Image>();
        ButtonImage.sprite = DefaultSprite;
    }

    public void OnPointerDown()
    {
        if (InGameManager.Instance.CurPlayer.isAlive)
        {
            ButtonImage.sprite = PressedSprite;
            InGameUIManager.Instance.OnPointerDown();

            pointerDown = true;
        }
    }

    public void OnPointerUp()
    {
        if (pointerDown)
        {
            ButtonImage.sprite = DefaultSprite;
            InGameUIManager.Instance.OnPointerUp();
        }
    }
}
