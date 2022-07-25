using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ChooseMessage : MonoBehaviour, IPopUp
{
    public Button AcceptButton;
    public Button RefuseButton;
    public Text MessageTxt;

    MessageBoxContainer manager;
    RectTransform rect;

    public void Init(Action accept, Action refuse, string message, MessageBoxContainer _manager)
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -1200f);
        rect.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutBack);

        manager = _manager;

        ShopUIManager.Instance.AddPopup(this);
        MessageTxt.text = message;

        AcceptButton.onClick.RemoveAllListeners();
        RefuseButton.onClick.RemoveAllListeners();

        refuse += Close;
        refuse += manager.ClosePopup;
        accept += manager.ClosePopup;

        AcceptButton.onClick.AddListener(() => accept?.Invoke());
        RefuseButton.onClick.AddListener(() => refuse?.Invoke());
    }

    public void Refuse()
    {
        ShopUIManager.Instance.PopupClose();
        Close();
    }

    public void Close()
    {
        rect.DOAnchorPosY(-1200f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
