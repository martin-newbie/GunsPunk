using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DroneSelect : MonoBehaviour, IPopUp
{
    public Button[] selectButtons = new Button[3];
    public Text[] itemsCount = new Text[3];
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    public Image Background;
    public int curSelected = -1;

    private void Start()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            int idx = i;
            selectButtons[i].onClick.AddListener(() => ButtonSelect(idx));
        }
    }

    public void UIOpen()
    {
        GameManager.Instance.AddPopup(this);

        for (int i = 0; i < itemsCount.Length; i++)
        {
            itemsCount[i].text = GameManager.Instance.itemsInfo[i + 1].count.ToString();
            if (GameManager.Instance.itemsInfo[i + 1].count <= 0) itemsCount[i].color = Color.red;
        }

        Background.rectTransform.anchoredPosition = new Vector2(0f, -1400f);
        Background.rectTransform.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutBack);
    }


    void ButtonSelect(int idx)
    {
        AudioManager.Instance.PlayUISound("SwitchClick");

        if (GameManager.Instance.itemsInfo[idx + 1].count <= 0)
        {
            MessageBoxContainer.Instance.OpenChooseMessage(
                () => {
                    PlayerPrefs.SetInt("ShopState", 1);
                    LoadingSceneManager.LoadScene("ShopScene"); 
                },
                null,
                "No item remain \n Do you want to buy this?"
            );
            return;
        }



        foreach (var item in selectButtons)
        {
            item.image.sprite = unselectedSprite;
        }

        if (idx == curSelected)
        {
            curSelected = -1;
        }
        else
        {
            curSelected = idx;
            selectButtons[idx].image.sprite = selectedSprite;
        }

        PlayerPrefs.SetInt("droneIdx", curSelected);
    }

    public void GameStart()
    {
        if(curSelected != -1)
        {
            GameManager.Instance.itemsInfo[curSelected + 1].count--;
        }

        GameManager.Instance.gamePlayCnt++;
        LoadingSceneManager.LoadScene("InGameScene");
    }

    public void CloseButton()
    {
        AudioManager.Instance.PlayUISound("SwitchClick");
        GameManager.Instance.PopupClose();
    }

    public void Close()
    {
        Background.rectTransform.DOAnchorPosY(-1400f, 0.5f).SetEase(Ease.InBack).OnComplete(()=> { gameObject.SetActive(false); });
    }
}
