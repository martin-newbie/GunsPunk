using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeKind
{
    HP,
    AMMO,
    DEF
}

public class UpgradeContainer : MonoBehaviour, IRefresh
{
    public Image[] UpgradeImage;

    public UpgradeKind kind;

    public Text LevelTxt;
    public Text CostTxt;
    public Text ProgressTxt;

    public Image UpgradeButton;
    public Image MaxButton;

    int cost => GameManager.Instance.GetUpgradeCost(Level);
    int Level => kind switch
    {
        UpgradeKind.HP => GameManager.Instance.hpLevel,
        UpgradeKind.AMMO => GameManager.Instance.ammoLevel,
        UpgradeKind.DEF => GameManager.Instance.defLevel,
        _ => 0,
    };

    bool upgradeAble => Level <= GameManager.Instance.maxLevel;



    private void Start()
    {
        ShopUIManager.Instance.AddRefreshAble(this);

        for (int i = 0; i < UpgradeImage.Length; i++)
        {
            if (i == (int)kind) UpgradeImage[i].gameObject.SetActive(true);
            else UpgradeImage[i].gameObject.SetActive(false);
        }

        Refresh();
    }

    public void Refresh()
    {
        float value = kind switch
        {
            UpgradeKind.HP => GameManager.Instance.hpValue,
            UpgradeKind.AMMO => GameManager.Instance.ammoValue,
            UpgradeKind.DEF => GameManager.Instance.defValue,
            _ => 0f,
        };
        float nextValue = upgradeAble ? value + 0.15f : 0f;

        if (upgradeAble)
        {
            ProgressTxt.text = string.Format("{0:0.00}", value) + "¡æ" + string.Format("{0:0.00}", nextValue);
            CostTxt.text = string.Format("{0:0,#}", cost);
        }
        else
        {
            ProgressTxt.text = string.Format("{0:0.00}", value);
            CostTxt.text = "Max";
        }
        LevelTxt.text = "Lv. " + Level.ToString();


        UpgradeButton.gameObject.SetActive(upgradeAble);
        MaxButton.gameObject.SetActive(!upgradeAble);
    }

    public void Upgrade()
    {
        if (upgradeAble && GameManager.Instance.coin >= cost)
        {
            AudioManager.Instance.PlayUISound("Purchase");
            GameManager.Instance.coin -= cost;

            switch (kind)
            {
                case UpgradeKind.HP:
                    GameManager.Instance.hpLevel++;
                    break;
                case UpgradeKind.AMMO:
                    GameManager.Instance.ammoLevel++;
                    break;
                case UpgradeKind.DEF:
                    GameManager.Instance.defLevel++;
                    break;
            }
        }

        ShopUIManager.Instance.Refresh();
    }
}
