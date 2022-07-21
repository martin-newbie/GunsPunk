using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public Button[] Buttons;
    public Sprite[] SelectButtonImg;
    public Sprite[] UnselectButtonImg;
    public GameObject[] Windows;

    private void Start()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            int idx = i;
            Buttons[i].onClick.AddListener(() => OpenWindow(idx));
        }

        OpenWindow(0);
    }

    void OpenWindow(int idx)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].image.sprite = UnselectButtonImg[i];
            Windows[i].SetActive(false);
        }

        Windows[idx].SetActive(true);
        Buttons[idx].image.sprite = SelectButtonImg[idx];
    }

}
