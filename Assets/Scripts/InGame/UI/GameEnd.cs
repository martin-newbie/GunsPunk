using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameEnd : MonoBehaviour
{
    bool already;

    [Header("Windows")]
    public GameObject ReviveWindow;
    public GameObject ResultWindow;
    public RectTransform ReviveMain;
    public RectTransform ResultMain;

    [Header("UI Objects")]
    public RectTransform secondNeedle;

    Coroutine revive;

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

    }

    IEnumerator ReviveCoroutine(float duration)
    {
        yield return ReviveMain.DOAnchorPosY(40, 0.5f).SetEase(Ease.OutBack);

        float timer = duration;
        while (timer > 0f)
        {



            timer -= Time.deltaTime;
            yield return null;
        }


        yield break;
    }

    public void ButtonSkip()
    {
        StopCoroutine(revive);
        OpenResult();
    }
}
