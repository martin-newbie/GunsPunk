using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : Singleton<InGameUIManager>
{
    public Image PlayerHpGauge;
    public Image FeverGauge;

    float hpCur, hpMax, hpTarget;
    float feverCur, feverMax, feverTarget;

    private void Update()
    {
        hpCur = Mathf.Lerp(hpCur, hpTarget, Time.deltaTime * 10f);
        feverCur = Mathf.Lerp(feverCur, feverTarget, Time.deltaTime * 10f);

        PlayerHpGauge.fillAmount = hpCur / hpMax;
        FeverGauge.fillAmount = feverCur / feverMax;
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

    public void OnPointerDown()
    {
        InGameManager.Instance.CurPlayer.OnAttackStart();
    }

    public void OnPointerUp()
    {
        InGameManager.Instance.CurPlayer.OnAttackEnd();
    }
}
