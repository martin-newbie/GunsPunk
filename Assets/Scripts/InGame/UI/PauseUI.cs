using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour, IPopUp
{
    public void Init()
    {
        GameManager.Instance.AddPopup(this);
    }

    public void ButtonQuit()
    {
        Time.timeScale = 1f;
        LoadingSceneManager.LoadScene("MainScene");
    }

    public void ButtonRetry()
    {
        Time.timeScale = 1f;
        LoadingSceneManager.LoadScene("InGameScene");
    }

    public void ButtonResume()
    {
        GameManager.Instance.PopupClose();
    }

    public void Close()
    {
        Time.timeScale = 1f;
        InGameUIManager.Instance.PauseOff();
    }
}
