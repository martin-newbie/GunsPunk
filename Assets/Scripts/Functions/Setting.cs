using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Setting : Singleton<Setting>, IPopUp
{
    public Image mainObject;
    public Slider BGM;
    public Slider UI;
    public Slider GAME;

    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void ValueChangeBGM()
    {
        AudioManager.instance.volumeBGM = BGM.value;
    }

    public void ValueChangeUI()
    {
        AudioManager.instance.volumeUI = UI.value;
    }

    public void ValueChangeGAME()
    {
        AudioManager.instance.volumeGAME = GAME.value;
    }

    public void UIOn()
    {
        GameManager.Instance.AddPopup(this);

        BGM.value = AudioManager.instance.volumeBGM;
        UI.value = AudioManager.instance.volumeUI;
        GAME.value = AudioManager.instance.volumeGAME;
        mainObject.rectTransform.anchoredPosition = new Vector2(0f, -1500f);
        mainObject.rectTransform.DOAnchorPosY(-12f, 0.5f).SetEase(Ease.OutBack);
    }

    public void UIOff()
    {
        GameManager.Instance.PopupClose();
    }

    public void ButtonTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 0);
        LoadingSceneManager.LoadScene("InGameScene");
    }

    public void ButtonCredit()
    {

    }

    public void ButtonGoogle()
    {

    }

    public void Close()
    {
        mainObject.rectTransform.DOAnchorPosY(-1500f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
