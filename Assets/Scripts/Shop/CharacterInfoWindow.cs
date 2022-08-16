using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour, IPopUp, IRefresh
{
    public CharacterInfo info;

    [Header("Sprites")]
    public Sprite[] CharacterIllustSprites;
    public Sprite[] CharacterNameSprites;
    public Sprite selectSprite;
    public Sprite selectedSprite;

    [Header("UI Objects")]
    public RadialGraph CharacterStatusGraph;
    public Image[] Stars;
    public Image CharacterLevelGauge;
    public Image CharacterIllustImage;
    public Image CharacterName;
    public Text CharacterLevel;
    public Text CharacterDesc;
    public Button TrainingButton;
    public Button ChooseButton;

    public void WindowOpen(CharacterInfo _info)
    {
        AudioManager.Instance.PlayUISound("InfoOpen");

        GameManager.Instance.AddPopup(this);
        ShopUIManager.Instance.AddRefreshAble(this);

        int idx = 0;
        foreach (var item in Stars)
        {
            if (idx < _info.trainingLevel)
            {
                item.gameObject.SetActive(true);
            }
            else item.gameObject.SetActive(false);
            idx++;
        }
        // todo
        CharacterIllustImage.sprite = CharacterIllustSprites[_info.idx];
        CharacterName.sprite = CharacterNameSprites[_info.idx];
        CharacterLevel.text = _info.level.ToString();
        CharacterLevelGauge.fillAmount = _info.exp / _info.maxExp;
        CharacterDesc.text = _info.description; 
        info = _info;

        SetRadialGraph(GameManager.Instance.GetCharacterInfo(_info.idx));
        CheckTrainingAble();
        CheckSelected();
        ActiveStars();
    }

    void CheckSelected()
    {
        ChooseButton.image.sprite = info.isSelected ? selectedSprite : selectSprite;
    }

    void CheckTrainingAble()
    {
        TrainingButton.gameObject.SetActive(info.TrainigAble());
    }

    void ActiveStars()
    {
        for (int i = 0; i < Stars.Length; i++)
        {
            if(i < info.trainingLevel)
            {
                Stars[i].gameObject.SetActive(true);
            }
            else
            {
                Stars[i].gameObject.SetActive(false);
            }
        }
    }

    public void CharacterChoose()
    {
        AudioManager.Instance.PlayUISound("SwitchClick");
        if (!info.isSelected)
        {
            // select main or sub
            ShopUIManager.Instance.ChooseCharacterPos(info);
        }
        else
        {
            MessageBoxContainer.Instance.OpenConfirmMessage(null, "Already Selected");
        }

    }

    public void CharacterTraining()
    {
        if (info.TrainigAble())
        {
            // open popup

            if (info.trainingLevel < info.maxTrainingLevel)
            {
                // use coin
            }
            else
            {
                // use energy
            }

            info.trainingLevel++;
        }
        else
        {
            AudioManager.Instance.PlayUISound("Error");
            MessageBoxContainer.Instance.OpenConfirmMessage(null, "Not enought coin");
        }
        CheckTrainingAble();

        ShopUIManager.Instance.Refresh();
    }

    void SetRadialGraph(characterInfo info)
    {
        CharacterStatusGraph.top = info.hp / 250f;
        CharacterStatusGraph.rightTop = info.ammo / 250f;
        CharacterStatusGraph.rightBot = info.damage / 30f;
        CharacterStatusGraph.leftBot = info.fever / 25f;
        CharacterStatusGraph.leftTop = info.rpm / 2500f;
    }

    public void WindowClose()
    {
        GameManager.Instance.PopupClose();
    }

    public void Close()
    {
        AudioManager.Instance.PlayUISound("InfoClose");
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        if (info != null)
        {
            CheckTrainingAble();
            SetRadialGraph(GameManager.Instance.GetCharacterInfo(info.idx));
            CheckSelected();
            ActiveStars();
        }
    }
}
