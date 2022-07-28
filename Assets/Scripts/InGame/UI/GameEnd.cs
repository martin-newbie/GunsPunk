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
    public Image UserLevelGauge;
    public Image CharacterLevelGauge;
    public Text UserLevel;
    public Text CharacterLevel;
    public Text CharacterName;
    public Text RoundCoin;
    public Text RoundDistance;
    public Text ExpAmount;

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
        result = StartCoroutine(ResultCoroutine(reward.charLevel, reward.charExp, reward.userLevel, reward.userExp, reward.roundDistance, reward.roundCoin, reward.roundExp));
    }

    IEnumerator ResultCoroutine(int charLevel, float charExp, int userLevel, float userExp, float _distance, int _coin, float _exp)
    {

        CharacterInfo curChar = GameManager.Instance.GetMainPlayer();

        // init part
        {
            CharacterInfo info = GameManager.Instance.GetMainPlayer();

            UserLevelGauge.fillAmount = userExp / GameManager.Instance.GetUserMaxExp(userLevel);
            UserLevel.text = "Lv." + userLevel.ToString();

            CharacterLevelGauge.fillAmount = charExp / info.GetMaxExp(charLevel);
            CharacterLevel.text = "Lv." + charLevel;

            RoundCoin.text = "0";
            RoundDistance.text = "0";

            ExpAmount.text = "";

            CharacterName.text = info.characterName;
        }

        ResultWindow.SetActive(true);
        yield return ResultMain.DOAnchorPosY(-50f, 0.5f).SetEase(Ease.OutBack);

        // user levelUp
        {
            float curExp = userExp;
            int curLevel = userLevel;

            float timer = 2f;
            float offset = _exp / timer;

            while (timer > 0f)
            {
                curExp += offset * Time.deltaTime;
                UserLevelGauge.fillAmount = curExp / GameManager.Instance.GetUserMaxExp(curLevel);
                UserLevel.text = "Lv." + curLevel.ToString();

                if(curExp >= GameManager.Instance.GetUserMaxExp(curLevel))
                {
                    curExp -= GameManager.Instance.GetUserMaxExp(curLevel);
                    curLevel++;
                }

                timer -= Time.deltaTime;
                yield return null;
            }
        }


        // character levelUp
        {
            int curLevel = charLevel;
            float curExp = charExp;

            float timer = 2f;
            float offset = _exp / timer;

            while (timer > 0f)
            {
                curExp += offset * Time.deltaTime;
                CharacterLevel.text = "Lv." + curLevel.ToString();
                CharacterLevelGauge.fillAmount = curExp / curChar.GetMaxExp(curLevel);

                if (curExp >= curChar.GetMaxExp(curLevel))
                {
                    curExp -= curChar.GetMaxExp(curLevel);
                    curLevel++;
                }

                timer -= Time.deltaTime;
                yield return null;
            }
        }
        

        yield return StartCoroutine(TextCounting(RoundCoin, _coin, 1f));
        yield return StartCoroutine(TextCounting(RoundDistance, _distance, 1f));

        yield break;
    }

    IEnumerator TextCounting(Text text, float target, float duration)
    {
        float timer = duration;
        float offset = target / duration;
        float current = 0f;

        while (timer > 0f)
        {
            current += offset * Time.deltaTime;
            text.text = string.Format("{0:#,0}", current);

            timer -= Time.deltaTime;
            yield return null;
        }

        current = target;
        text.text = string.Format("{0:#,0}", current);
        yield break;
    }

    IEnumerator ReviveCoroutine(float duration)
    {
        ReviveWindow.SetActive(true);
        yield return ReviveMain.DOAnchorPosY(40, 0.5f).SetEase(Ease.OutBack);

        float timer = duration;
        while (timer > 0f)
        {
            secondNeedle.rotation = Quaternion.Euler(0, 0, 360 * (timer / duration));
            timer -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        ButtonSkip();
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

    void Skip()
    {
        ReviveMain.DOAnchorPosY(-1200f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            ReviveWindow.SetActive(false);
        });
        OpenResult();
    }

    public void ButtonSkip()
    {
        StopCoroutine(revive);
        Skip();
    }

    public void ButtonHome()
    {
        LoadingSceneManager.LoadScene("MainScene");
    }

    public void ButtonReplay()
    {
        LoadingSceneManager.LoadScene("InGameScene");
    }
}
