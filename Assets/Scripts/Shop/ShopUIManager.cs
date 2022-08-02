using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IPopUp
{
    void Close();
}

public interface IRefresh
{
    void Refresh();
}

public class ShopUIManager : Singleton<ShopUIManager>
{
    public List<IRefresh> RefreshAble = new List<IRefresh>();

    [Header("PopUp")]
    public CharacterInfoWindow infoWindow;

    [Header("UI")]
    public Button[] Buttons;
    public Sprite[] SelectButtonImg;
    public Sprite[] UnselectButtonImg;
    public GameObject[] Windows;
    public CharacterContainer CharacterPrefab;

    [Header("Windows")]
    public RectTransform CharacterContents;
    public ChooseCharacterPosition ChoosePos;

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
    }

    private void Update()
    {
    }

    public void ChooseCharacterPos(CharacterInfo info)
    {
        ChoosePos.gameObject.SetActive(true);
        ChoosePos.Init(info);
    }

    public void AddRefreshAble(IRefresh refresh)
    {
        RefreshAble.Add(refresh);
    }

    public void Refresh()
    {
        foreach (var item in RefreshAble)
        {
            item.Refresh();
        }
    }

    public void OpenCharacterInfo(CharacterInfo info)
    {
        infoWindow.gameObject.SetActive(true);
        infoWindow.WindowOpen(info);
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
