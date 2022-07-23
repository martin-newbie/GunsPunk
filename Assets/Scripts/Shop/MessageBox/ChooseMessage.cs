using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChooseMessage : MonoBehaviour, IPopUp
{
    public Button AcceptButton;
    public Button RefuseButton;
    public Text MessageTxt;

    MessageBoxContainer manager;

    public void Init(Action accept, Action refuse, string message, MessageBoxContainer _manager)
    {
        manager = _manager;

        ShopUIManager.Instance.AddPopup(this);
        MessageTxt.text = message;

        AcceptButton.onClick.RemoveAllListeners();
        RefuseButton.onClick.RemoveAllListeners();

        refuse += Close;
        refuse += manager.ClosePopup;
        accept += manager.ClosePopup;

        AcceptButton.onClick.AddListener(()=> accept?.Invoke());
        RefuseButton.onClick.AddListener(() => refuse?.Invoke());
    }

    public void Refuse()
    {
        ShopUIManager.Instance.PopupClose();
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
