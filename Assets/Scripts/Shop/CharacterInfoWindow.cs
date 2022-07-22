using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour, IPopUp
{

    public RadialGraph CharacterStatusGraph;
    public Image[] Stars;
    public Image CharacterLevelGauge;
    public Image CharacterIllustImage;
    public Text CharacterLevel;
    public Text CharacterName;
    public Text CharacterDesc;

    public void WindowOpen(CharacterInfo info)
    {
        ShopUIManager.Instance.AddPopup(this);

        int idx = 0;
        foreach (var item in Stars)
        {
            if (idx < info.level)
            {
                item.gameObject.SetActive(true);
            }
            else item.gameObject.SetActive(false);
            idx++;
        }
        // todo
        CharacterIllustImage.sprite = null; // get illust by character's index
        CharacterLevelGauge.fillAmount = 0f; //get exp by character's exp / maxExp
        CharacterLevel.text = "Lv. " + info.level.ToString();
        CharacterName.text = ""; //get name by character's name text asset
        CharacterDesc.text = ""; //get description by character's description text asset

        SetRadialGraph(GameManager.Instance.GetCharacterInfo(info.idx));
    }

    void SetRadialGraph(characterInfo info)
    {
        CharacterStatusGraph.top = info.hp / 500f;
        CharacterStatusGraph.rightTop = info.ammo / 500f;
        CharacterStatusGraph.rightBot = info.damage / 50f;
        CharacterStatusGraph.leftBot = info.fever / 25f;
        CharacterStatusGraph.leftTop = info.rpm / 2500f;
    }

    public void WindowClose()
    {
        ShopUIManager.Instance.PopupClose();
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
