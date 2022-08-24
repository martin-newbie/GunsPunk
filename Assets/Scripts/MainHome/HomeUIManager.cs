using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    [Header("Divided UI")]
    public HomeQuest Quest;
    public HomeBestScore BestScore;
    public HomeProfile Profile;
    public DroneSelect droneSelect;

    void Start()
    {
        CameraResolution.SetCameraResolution();
        Quest.Init(this);
        BestScore.Init(this);
        Profile.Init(this);

        SetValueText();
    }

    void SetValueText()
    {
        BestScore.SetUI(string.Format("{0:#,0}", GameManager.Instance.bestScore));
        Profile.SetUI(GameManager.Instance.userName, GameManager.Instance.userLevel, null);
    }

    public void GameStart()
    {
        if(GameManager.Instance.itemsInfo[1].count > 0 || GameManager.Instance.itemsInfo[2].count > 0 || GameManager.Instance.itemsInfo[3].count > 0)
        {
            droneSelect.gameObject.SetActive(true);
            droneSelect.UIOpen();
        }
        else
        {
            GameManager.Instance.gamePlayCnt++;
            LoadingSceneManager.LoadScene("InGameScene");
        }
    }

    public void Shop()
    {
        LoadingSceneManager.LoadScene("ShopScene");
    }
}
