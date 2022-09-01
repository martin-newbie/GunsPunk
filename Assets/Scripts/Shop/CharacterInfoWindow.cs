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

    [Header("Character Training")]
    public Text costTxt;
    public GameObject[] icons = new GameObject[2];
    public ValueType trainingType;
    public int value
    {
        get
        {
            return trainingType switch
            {
                ValueType.Coin => GameManager.Instance.coin,
                ValueType.Energy => GameManager.Instance.energy,
                _ => throw new System.NotImplementedException(),
            };
        }
        set
        {
            switch (trainingType)
            {
                case ValueType.Coin:
                    GameManager.Instance.coin = value;
                    break;
                case ValueType.Energy:
                    GameManager.Instance.energy = value;
                    break;
            }
        }
    }

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
        CharacterIllustImage.SetNativeSize();
        
        CharacterName.sprite = CharacterNameSprites[_info.idx];
        CharacterName.SetNativeSize();

        CharacterLevel.text = _info.level.ToString();
        CharacterLevelGauge.fillAmount = _info.exp / _info.maxExp;
        CharacterDesc.text = _info.description;
        info = _info;

        Refresh();
    }

    void CheckSelected()
    {
        ChooseButton.image.sprite = info.isSelected ? selectedSprite : selectSprite;
    }

    void CheckTrainingAble()
    {
        if (info.TrainingAble())
        {
            if (info.trainingLevel < info.maxTrainingLevel - 1)
            {
                // use coin
                icons[0].SetActive(true);
                icons[1].SetActive(false);
                trainingType = ValueType.Coin;
            }
            else
            {
                // use energy
                icons[0].SetActive(false);
                icons[1].SetActive(true);
                trainingType = ValueType.Energy;
            }
            costTxt.text = string.Format("{0:0,#}", GameManager.Instance.TrainingCost[info.trainingLevel]);
            costTxt.color = GameManager.Instance.TrainingCost[info.trainingLevel] <= value ? Color.white : Color.red;
        }
        TrainingButton.gameObject.SetActive(info.TrainingAble());
    }

    void ActiveStars()
    {
        for (int i = 0; i < Stars.Length; i++)
        {
            if (i < info.trainingLevel)
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
        if (value >= GameManager.Instance.TrainingCost[info.trainingLevel])
        {
            // open popup
            value -= GameManager.Instance.TrainingCost[info.trainingLevel];
            info.trainingLevel++;
            AudioManager.Instance.PlayUISound("Purchase");
            AudioManager.Instance.PlayUISound("Training");
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
        CharacterStatusGraph.leftTop = info.rpm / 1000f;
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
