using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopAD : MonoBehaviour
{
    public Sprite[] Packages;

    public Image front;
    public Image back;

    public int idx = 0;
    public int sub = 1;

    void Start()
    {
        StartCoroutine(ADmove());
    }

    private void Update()
    {
        front.sprite = Packages[idx];
        back.sprite = Packages[sub];
    }

    public void MoveToShop()
    {
        LoadingSceneManager.LoadScene("ShopScene");
        PlayerPrefs.SetInt("ShopState", 3);
    }

    IEnumerator ADmove()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            yield return front.rectTransform.DOAnchorPosX(-750f, 1f).OnComplete(() =>
            {
                front.rectTransform.anchoredPosition = Vector2.zero;
                idx = sub;

                if (idx < Packages.Length - 1) sub = idx + 1;
                else sub = 0;
            });

            yield return null;
        }
    }
}
