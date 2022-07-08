using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : Singleton<InGameUIManager>
{
    public Image PlayerHpGauge;
    public Image FeverGauge;

    float hpCur, hpMax;
    float feverCur, feverMax;

    private void Update()
    {
        PlayerHpGauge.fillAmount = Mathf.Lerp(PlayerHpGauge.fillAmount, hpCur, Time.deltaTime * 15f) / hpMax;
        FeverGauge.fillAmount = Mathf.Lerp(FeverGauge.fillAmount, feverCur, Time.deltaTime * 15f) / feverMax;
    }

    public void SetPlayerHp(float cur, float max)
    {
        hpCur = cur;
        hpMax = max;
    }

    public void SetFeverGauge(float cur, float max)
    {
        feverCur = cur;
        feverMax = max;
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
