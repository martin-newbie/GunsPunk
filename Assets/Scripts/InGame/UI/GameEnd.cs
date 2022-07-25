using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameEnd : MonoBehaviour
{
    bool already;

    [Header("Windows")]
    public GameObject ReviveWindow;
    public GameObject ResultWindow;
    public RectTransform ReviveMain;
    public RectTransform ResultMain;

    [Header("Revive Object")]
    public RectTransform secondNeedle;

    [Header("Result Object")]


    Coroutine revive;
    Coroutine result;

    public void OpenGameEnd()
    {
        ReviveWindow.SetActive(false);
        ResultWindow.SetActive(false);

        ReviveMain.anchoredPosition = new Vector2(0, -1200);
        ResultMain.anchoredPosition = new Vector2(0, -1200);



        if (!already)
        {
            OpenRevive();
        }
        else
        {
            OpenResult();
        }
    }

    void OpenRevive()
    {
        revive = StartCoroutine(ReviveCoroutine(15f));
    }

    void OpenResult()
    {
        var reward = InGameManager.Instance.GetResult();
        result = StartCoroutine(ResultCoroutine(reward._distance, reward._coin));
    }

    IEnumerator ResultCoroutine(float _distance, int _coin)
    {
        yield break;
    }

    IEnumerator ReviveCoroutine(float duration)
    {
        ReviveWindow.SetActive(true);
        yield return ReviveMain.DOAnchorPosY(40, 0.5f).SetEase(Ease.OutBack);

        float timer = duration;
        while (timer > 0f)
        {
            secondNeedle.rotation = Quaternion.Euler(0, 0, 90 + (360 * (timer / duration)));
            timer -= Time.deltaTime;
            yield return null;
        }

        OpenResult();
        yield break;
    }

    public void ButtonRevive()
    {
        StopCoroutine(revive);
        ReviveMain.DOAnchorPosY(-1200f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            already = true;
            ReviveWindow.SetActive(false);
            InGameManager.Instance.Revive();
        });
    }

    public void ButtonSkip()
    {
        StopCoroutine(revive);
        OpenResult();
    }
}
