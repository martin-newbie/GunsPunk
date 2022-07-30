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
    public CharacterInfo info;

    public Image CharacterIllustImage;
    public Image CharacterProfileImage;
    public Image CharacterNameImage;

    public GameObject LockedObject;
    public GameObject[] ValueIcons;
    public GameObject[] Stars;
    public Text Cost;

    [Header("Button")]
    public Button selectButton;
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    public GameObject UnlockedObject;

    public void Init(int idx)
    {
        ShopUIManager.Instance.AddRefreshAble(this);

        CharacterIllustImage.sprite = ShopUIManager.Instance.CharactersIllustSprite[idx];
        CharacterProfileImage.sprite = ShopUIManager.Instance.CharactersProfileSprite[idx];
        CharacterNameImage.sprite = ShopUIManager.Instance.CharactersNameSprite[idx];

        info = GameManager.Instance.charactersInfo[idx];


        
        Refresh();
    }

    public void Refresh()
    {
        if (info.isUnlocked)
        {
            LockedObject.SetActive(false);
            UnlockedObject.SetActive(true);

            for (int i = 0; i < Stars.Length; i++)
            {
                if (i < info.trainingLevel)
                    Stars[i].SetActive(true);
                else Stars[i].SetActive(false);
            }
        }
        else
        {
            LockedObject.SetActive(true);
            UnlockedObject.SetActive(false);
            Cost.text = string.Format("{0:#,##}", info.cost);

            ValueIcons[(int)info.valueType].SetActive(true);

            if (GameManager.Instance.curCoin < info.cost)
                Cost.color = Color.red;
            else
                Cost.color = Color.white;

        }


        selectButton.image.sprite = info.isSelected ? selectedSprite : defaultSprite;
    }

    public void Select()
    {
        if (!info.isSelected)
            ShopUIManager.Instance.ChooseCharacterPos(info);
        else
            MessageBoxContainer.Instance.OpenConfirmMessage(null, "Already Selected");

    }

    public void ButtonInfo()
    {
        ShopUIManager.Instance.OpenCharacterInfo(info);
    }

    public void ButtonRecruit()
    {
        bool recruitAble;
        switch (info.valueType)
        {
            case ValueType.Coin:
                recruitAble = GameManager.Instance.curCoin >= info.cost;
                break;
            case ValueType.Energy:
                recruitAble = GameManager.Instance.energy >= info.cost;
                break;
            default:
                recruitAble = false;
                break;
        }

        if (recruitAble)
        {
            switch (info.valueType)
            {
                case ValueType.Coin:
                    GameManager.Instance.curCoin -= info.cost;
                    break;
                case ValueType.Energy:
                    GameManager.Instance.energy -= info.cost;
                    break;
            }
            info.isUnlocked = true;
        }
        else
        {
            //print message
            //not enought money
            MessageBoxContainer.Instance.OpenConfirmMessage(null, "Not enough coin or energy");
        }

        ShopUIManager.Instance.Refresh();
    }
}
