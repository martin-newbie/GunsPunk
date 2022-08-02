using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestManager : Singleton<QuestManager>
{
    public string questSaveName = "QuestData";
    public List<QuestData> Quests = new List<QuestData>();

    private void Awake()
    {
        string saveData = PlayerPrefs.GetString(questSaveName, "None");
        if(saveData == "None")
        {
            // init quest
        }
        else
        {
            SetSaveData(saveData);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) PlayerPrefs.SetString(questSaveName, GetSaveData());
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(questSaveName, GetSaveData());
    }

    string GetSaveData()
    {
        string saveData = JsonUtility.ToJson(Quests, true);
        return saveData;
    }

    void SetSaveData(string saveData)
    {
        Quests = JsonUtility.FromJson<List<QuestData>>(saveData);
    }
}

public enum RewardType
{
    COIN,
    ENERGY
}

[System.Serializable]
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
[System.Serializable]
public class QuestSample : QuestData
{
    public override float curProgress => GameManager.Instance.bestScore;
}