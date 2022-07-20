using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
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
        Time.timeScale = 1f;
        InGameUIManager.Instance.PauseOff();
    }
}
