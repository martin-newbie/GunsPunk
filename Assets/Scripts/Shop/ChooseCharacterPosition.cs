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
        GameManager.Instance.AddPopup(this);
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
        AudioManager.Instance.PlayUISound("SetCharacter");
        ShopUIManager.Instance.Refresh();
        GameManager.Instance.PopupClose();
    }

    public void Close()
    {
        MainObj.DOAnchorPosY(-1200f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
