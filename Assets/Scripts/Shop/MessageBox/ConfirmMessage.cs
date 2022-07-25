using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ConfirmMessage : MonoBehaviour, IPopUp
{
    public Button ConfirmButton;
    public Text MessageTxt;

    MessageBoxContainer manager;
    RectTransform rect;

    public void Init(Action confirm, string message, MessageBoxContainer _manager)
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, -1200f);
        rect.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutBack);

        manager = _manager;
        ShopUIManager.Instance.AddPopup(this);
        MessageTxt.text = message;

        ConfirmButton.onClick.RemoveAllListeners();

        confirm += Confirm;
        confirm += manager.ClosePopup;

        ConfirmButton.onClick.AddListener(() => confirm?.Invoke());
    }

    public void Confirm()
    {
        ShopUIManager.Instance.PopupClose();
        Close();
    }

    public void Close()
    {
        rect.DOAnchorPosY(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
