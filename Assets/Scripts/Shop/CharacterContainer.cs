using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ValueType
{
    Coin,
    Energy
}

public class CharacterContainer : MonoBehaviour, IRefresh
{
    public CharacterInfo charInfo;

    public Image CharacterIllustImage;
    public Image CharacterProfileImage;
    public Image CharacterNameImage;

    public GameObject LockedObject;
    public GameObject[] ValueIcons;
    public GameObject[] Stars;
    public Text Cost;

    public GameObject UnlockedObject;

    public void Init(int idx)
    {
        ShopUIManager.Instance.AddRefreshAble(this);

        CharacterIllustImage.sprite = ShopUIManager.Instance.CharactersIllustSprite[idx];
        CharacterProfileImage.sprite = ShopUIManager.Instance.CharactersProfileSprite[idx];
        CharacterNameImage.sprite = ShopUIManager.Instance.CharactersNameSprite[idx];

        charInfo = GameManager.Instance.charactersInfo[idx];


        if (charInfo.isUnlocked)
        {
            LockedObject.SetActive(false);
            UnlockedObject.SetActive(true);
        }
        else
        {
            LockedObject.SetActive(true);
            UnlockedObject.SetActive(false);
            Cost.text = string.Format("{0:#,##}", charInfo.cost);

            ValueIcons[(int)charInfo.valueType].SetActive(true);

            if (GameManager.Instance.curCoin < charInfo.cost) Cost.color = Color.red;

        }

        for (int i = 0; i < charInfo.trainingLevel; i++)
        {
            Stars[i].SetActive(true);
        }
    }

    public void Refresh()
    {
        if (!charInfo.isUnlocked)
        {
            if (GameManager.Instance.curCoin < charInfo.cost)
                Cost.color = Color.red;
            else
                Cost.color = Color.white;
        }

        for (int i = 0; i < charInfo.trainingLevel; i++)
        {
            Stars[i].SetActive(true);
        }
    }

    public void ButtonInfo()
    {
        ShopUIManager.Instance.OpenCharacterInfo(charInfo);
    }

    public void ButtonRecruit()
    {
        if (GameManager.Instance.curCoin >= charInfo.cost)
        {
            GameManager.Instance.curCoin -= charInfo.cost;
            charInfo.isUnlocked = true;
        }
        else
        {
            //print message
            //not enought money
        }
    }
}
