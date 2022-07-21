using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : Singleton<ShopUIManager>
{
    [Header("UI")]
    public Button[] Buttons;
    public Sprite[] SelectButtonImg;
    public Sprite[] UnselectButtonImg;
    public GameObject[] Windows;
    public CharacterContainer CharacterPrefab;
    public Text CoinTxt;
    public Text EnergyTxt;

    [Header("Windows")]
    public RectTransform CharacterContents;

    [Header("Character Sprite")]
    public Sprite[] CharactersIllustSprite;
    public Sprite[] CharactersProfileSprite;
    public Sprite[] CharactersNameSprite;

    private void Start()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            int idx = i;
            Buttons[i].onClick.AddListener(() => OpenWindow(idx));
        }

        for (int i = 0; i < GameManager.Instance.charactersInfo.Length; i++)
        {
            CharacterContainer temp = Instantiate(CharacterPrefab, CharacterContents);
            temp.Init(i);
        }

        OpenWindow(0);
        SetValueText();
    }

    void SetValueText()
    {
        EnergyTxt.text = string.Format("{0:#,0}", GameManager.Instance.energy);
        CoinTxt.text = string.Format("{0:#,0}", GameManager.Instance.curCoin);
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

    public void BackToTitle()
    {
        LoadingSceneManager.LoadScene("MainScene");
    }
}
