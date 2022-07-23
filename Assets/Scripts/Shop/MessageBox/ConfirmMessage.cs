using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConfirmMessage : MonoBehaviour, IPopUp
{
    public Button ConfirmButton;
    public Text MessageTxt;

    MessageBoxContainer manager;

    public void Init(Action confirm, string message, MessageBoxContainer _manager)
    {
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
        gameObject.SetActive(false);
    }
}
