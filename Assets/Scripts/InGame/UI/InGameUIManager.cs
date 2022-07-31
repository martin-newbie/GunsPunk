using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : Singleton<InGameUIManager>
{

    [Header("UI Objects")]
    public Image PlayerHpGauge;
    public Image FeverGauge;
    public Image AmmoGauge;
    public Image HoldGauge;

    public Text RoundCoinText;

    [Header("Script Objects")]
    public PauseUI PauseObject;
    public GameEnd GameEndObject;

    int roundCoin;
    public int RoundCoin
    {
        get
        {
            return roundCoin;
        }
        set
        {
            roundCoin = value;
            RoundCoinText.text = string.Format("{0:#,##}", roundCoin);
        }
    }

    float hpCur, hpMax, hpTarget;
    float feverCur, feverMax, feverTarget;
    float ammoCur, ammoMax, ammoTarget;

    private void Start()
    {
        GameEndObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        hpCur = Mathf.Lerp(hpCur, hpTarget, Time.deltaTime * 10f);
        feverCur = Mathf.Lerp(feverCur, feverTarget, Time.deltaTime * 10f);
        ammoCur = Mathf.Lerp(ammoCur, ammoTarget, Time.deltaTime * 10f);

        PlayerHpGauge.fillAmount = hpCur / hpMax;
        FeverGauge.fillAmount = feverCur / feverMax;
        AmmoGauge.fillAmount = ammoCur / ammoMax;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            PauseOn();
    }

    public void StartGameEnd()
    {
        GameEndObject.gameObject.SetActive(true);
        GameEndObject.OpenGameEnd();
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        PauseObject.gameObject.SetActive(true);
        PauseObject.Init();
    }

    public void PauseOff()
    {
        PauseObject.gameObject.SetActive(false);
    }

    public void SetHoldGauge(float amt)
    {
        HoldGauge.fillAmount = amt;
    }

    public void SetPlayerHp(float cur, float max)
    {
        hpTarget = cur;
        hpMax = max;
    }

    public void SetFeverGauge(float cur, float max)
    {
        feverTarget = cur;
        feverMax = max;
    }

    public void SetAmmoGauge(float cur, float max)
    {
        ammoTarget = cur;
        ammoMax = max;
    }

    public void OnPointerDown()
    {
        InGameManager.Instance.CurPlayer.OnAttackStart();
    }

    public void OnPointerUp()
    {
        InGameManager.Instance.CurPlayer.OnAttackEnd();
    }
}
