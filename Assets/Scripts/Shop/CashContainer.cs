using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CashContainer : MonoBehaviour
{
    public Image bgImg;
    public Button buyButton;

    public void Init(Sprite common, Sprite pressed, Sprite bg, Action onClick = null)
    {
        bgImg.sprite = bg;
        buyButton.image.sprite = common;
        buyButton.onClick.AddListener(() => { onClick?.Invoke(); });

        SpriteState state = buyButton.spriteState;
        state.pressedSprite = pressed;
        buyButton.spriteState = state;
    }
}
