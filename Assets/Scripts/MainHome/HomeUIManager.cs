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
    }

    void Update()
    {
        BestScore.SetUI(GameManager.Instance.bestScore.ToString());
        Profile.SetUI(GameManager.Instance.playerName, GameManager.Instance.level, null);
    }

    public void GameStart()
    {

    }

    public void Shop()
    {

    }

    public void Setting()
    {

    }
}
