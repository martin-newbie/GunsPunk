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
    Action closeAction;

    public void Init(Action confirm, string message, MessageBoxContainer _manager)
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, -1200f);
        rect.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutBack);

        manager = _manager;
        GameManager.Instance.AddPopup(this);
        MessageTxt.text = message;

        ConfirmButton.onClick.RemoveAllListeners();

        confirm += Confirm;
        confirm += manager.ClosePopup;

        closeAction = confirm - Confirm;

        ConfirmButton.onClick.AddListener(() => confirm?.Invoke());
    }

    public void Confirm()
    {
        GameManager.Instance.PopupClose();
    }

    public void Close()
    {
        rect.DOAnchorPosY(-1200f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            closeAction?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
