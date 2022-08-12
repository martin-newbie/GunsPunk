using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MessageBoxContainer : Singleton<MessageBoxContainer>
{
    public ChooseMessage Choose;
    public ConfirmMessage Confirm;
    public GameObject Background;

    public void OpenChooseMessage(Action accept, Action refuse, string message)
    {
        Background.SetActive(true);
        Choose.gameObject.SetActive(true);
        Choose.Init(accept, refuse, message, this);
    }

    public void OpenConfirmMessage(Action confirm, string message)
    {
        Background.SetActive(true);
        Confirm.gameObject.SetActive(true);
        Confirm.Init(confirm, message, this);
    }

    public void ClosePopup()
    {
        Background.SetActive(false);
    }
}
