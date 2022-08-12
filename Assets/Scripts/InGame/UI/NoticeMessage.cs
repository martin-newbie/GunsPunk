using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NoticeMessage : MonoBehaviour
{
    public Image Back;
    public Text Text;

    private void Start()
    {
        Back.color = new Color(0, 0, 0, 0);
        Text.color = new Color(1, 1, 1, 0);
    }

    public void ShowMessage(string message)
    {
        Text.text = message;

        Text.DOColor(Color.white, 0.25f);
        Back.DOColor(new Color(0, 0, 0, 0.4f), 0.25f).OnComplete(() =>
        {
            Text.DOColor(new Color(1, 1, 1, 0), 0.25f).SetDelay(2f);
            Back.DOColor(new Color(0, 0, 0, 0), 0.25f).SetDelay(2f);
        });
    }

}
