using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestManager : MonoBehaviour
{
}

public enum RewardType
{
    COIN,
    ENERGY
}

public abstract class QuestData
{
    public string questName;
    public string questDesc;
    public RewardType rewardType;
    public int rewardValue;

    public abstract float curProgress { get; }
    public float maxProgress;
    public bool isQuestClear => curProgress >= maxProgress;
}


// sample
public class QuestSample : QuestData
{
    public override float curProgress => GameManager.Instance.bestScore;
}