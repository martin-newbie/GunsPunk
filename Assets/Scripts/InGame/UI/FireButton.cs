using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireButton : MonoBehaviour
{
    Image ButtonImage;

    public Sprite PressedSprite;
    public Sprite DefaultSprite;

    void Start()
    {
        ButtonImage = GetComponent<Image>();
        ButtonImage.sprite = DefaultSprite;
    }

    public void OnPointerDown()
    {
        ButtonImage.sprite = PressedSprite;
        InGameUIManager.Instance.OnPointerDown();
    }

    public void OnPointerUp()
    {
        ButtonImage.sprite = DefaultSprite;
        InGameUIManager.Instance.OnPointerUp();
    }
}
