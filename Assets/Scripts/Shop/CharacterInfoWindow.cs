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
        ShopUIManager.Instance.AddPopup(this);
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
        CharacterLevelGauge.fillAmount = 0f; //get exp by character's exp / maxExp
        CharacterDesc.text = ""; //get description by character's description text asset
        info = _info;

        SetRadialGraph(GameManager.Instance.GetCharacterInfo(_info.idx));
        CheckTrainingAble();
        CheckSelected();
    }

    void CheckSelected()
    {
        ChooseButton.image.sprite = info.isSelected ? selectedSprite : selectSprite;
    }

    void CheckTrainingAble()
    {
        TrainingButton.gameObject.SetActive(info.TrainigAble());
    }

    public void CharacterChoose()
    {
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

            if (info.trainingLevel < 4)
            {
                // use coin
            }
            else
            {
                // use energy
            }

            info.trainingLevel++;
        }
        CheckTrainingAble();

        ShopUIManager.Instance.Refresh();
    }

    void SetRadialGraph(characterInfo info)
    {
        CharacterStatusGraph.top = info.hp / 250f;
        CharacterStatusGraph.rightTop = info.ammo / 500f;
        CharacterStatusGraph.rightBot = info.damage / 30f;
        CharacterStatusGraph.leftBot = info.fever / 10f;
        CharacterStatusGraph.leftTop = info.rpm / 2500f;
    }

    public void WindowClose()
    {
        ShopUIManager.Instance.PopupClose();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        if (info != null)
        {
            CheckTrainingAble();
            SetRadialGraph(GameManager.Instance.GetCharacterInfo(info.idx));
            CheckSelected();
        }
    }
}
