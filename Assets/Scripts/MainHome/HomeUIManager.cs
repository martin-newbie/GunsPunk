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

    void Start()
    {
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
        GameManager.Instance.gamePlayCnt++;
        LoadingSceneManager.LoadScene("InGameScene");
    }

    public void Shop()
    {
        LoadingSceneManager.LoadScene("ShopScene");
    }
}
