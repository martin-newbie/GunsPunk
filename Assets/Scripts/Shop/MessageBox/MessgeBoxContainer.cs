using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MessgeBoxContainer : MonoBehaviour
{
    public ChooseMessage Choose;
    public ConfirmMessage Confirm;

    public void OpenChooseMessage(Action accept, Action refuse, string message)
    {
        Choose.gameObject.SetActive(true);
        Choose.Init(accept, refuse, message);
    }

    public void OpenConfirmMessage(Action confirm, string message)
    {
        Confirm.gameObject.SetActive(true);
        Confirm.Init(confirm, message);
    }
}
