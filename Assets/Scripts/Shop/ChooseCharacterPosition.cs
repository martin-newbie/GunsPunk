using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChooseCharacterPosition : MonoBehaviour, IPopUp
{

    public CharacterInfo info;
    public RectTransform MainObj;
    RectTransform rect;

    public void Init(CharacterInfo _info)
    {
        ShopUIManager.Instance.AddPopup(this);
        if (rect == null) rect = GetComponent<RectTransform>();

        MainObj.anchoredPosition = new Vector2(0, -1200f);
        MainObj.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack);

        info = _info;
    }

    public void ButtonMain()
    {
        GameManager.Instance.SetMainCharacter(info.idx);
        UIClose();
    }

    public void ButtonSub()
    {
        GameManager.Instance.SetSubCharacter(info.idx);
        UIClose();
    } 

    void UIClose()
    {
        ShopUIManager.Instance.Refresh();
        MainObj.DOAnchorPosY(-1200f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void Close()
    {
        UIClose();
    }
}
