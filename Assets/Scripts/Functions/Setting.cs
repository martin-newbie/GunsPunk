using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Setting : Singleton<Setting>, IPopUp
{
    public Image mainObject;

    private void Awake()
    {
        instance = this;

        gameObject.SetActive(false);
    }

    public void UIOn()
    {
        GameManager.Instance.AddPopup(this);

        mainObject.rectTransform.anchoredPosition = new Vector2(0f, -1500f);
        mainObject.rectTransform.DOAnchorPosY(-12f, 0.5f).SetEase(Ease.OutBack);
    }

    public void UIOff()
    {
        GameManager.Instance.PopupClose();
    }

    public void ButtonTutorial()
    {

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
