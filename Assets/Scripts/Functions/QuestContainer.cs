using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestContainer : MonoBehaviour, IRefresh
{
    public ValueType valueType;
    public QuestData data;

    [Header("Quest UI")]
    public Text questName;
    public Text questDesc;
    public Text progress;
    public Image progressImg;
    public Text cost;
    public Image energyIcon;
    public Image coinIcon;

    [Header("Reward UI")]
    public Button rewardButton;
    public Text rewardCost;
    public Image rewardEnergyIcon;
    public Image rewardCoinIcon;

    [Header("ETC")]
    public GameObject d;

    public void Init(QuestData _data)
    {
        data = _data;

        valueType = data.rewardType;

        questName.text = data.questName;
        questDesc.text = data.questDesc;
        progress.text = format(data.curProgress, "{0:0}") + "/" + format(data.maxProgress, "{0:0}");
        progressImg.fillAmount = data.curProgress / data.maxProgress;
        cost.text = format(data.rewardValue);

        energyIcon.gameObject.SetActive(valueType == ValueType.Energy);
        coinIcon.gameObject.SetActive(valueType == ValueType.Coin);
        rewardEnergyIcon.gameObject.SetActive(valueType == ValueType.Energy);
        rewardCoinIcon.gameObject.SetActive(valueType == ValueType.Coin);

        rewardButton.gameObject.SetActive(data.isQuestClear && data.isRewardAble);
        rewardCost.text = format(data.rewardValue);

    }

    public void GetReward()
    {
        switch (data.rewardType)
        {
            case ValueType.Coin:
                break;
            case ValueType.Energy:
                break;
        }

        data.isRewardAble = false;
        Refresh();
    }

    public void Refresh()
    {
        rewardButton.gameObject.SetActive(data.isQuestClear && data.isRewardAble);
    }

    string format(object args, string format = "{0:0,#}")
    {
        return string.Format(format, args);
    }
}
