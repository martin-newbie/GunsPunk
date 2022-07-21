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
        int idx = 0;
        foreach (var item in Buttons)
        {
            item.onClick.AddListener(() => OpenWindow(idx));
            idx++;
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
