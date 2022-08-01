using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public Text EnergyTxt;
    public Text CoinTxt;

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
        EnergyTxt.text = string.Format("{0:#,0}", GameManager.Instance.energy);
        CoinTxt.text = string.Format("{0:#,0}", GameManager.Instance.curCoin);
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

    public void Setting()
    {

    }
}
