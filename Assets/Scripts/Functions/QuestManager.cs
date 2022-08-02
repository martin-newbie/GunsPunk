using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class QuestManager : Singleton<QuestManager>
{
    public string questSaveName = "QuestData";
    public List<QuestData> Quests = new List<QuestData>();
    public QuestDataSaveList saveDataList = new QuestDataSaveList();
    public bool removeSave;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        string saveData = PlayerPrefs.GetString(questSaveName, "None");
        if(saveData == "None")
        {
            // init quest
            Quests.Add(new QuestSample("BestScore!", "Get Over BestScore", ValueType.Coin, 100, 50));
            Quests.Add(new QuestSample("BestScore!!", "Get Over BestScore", ValueType.Coin, 200, 100));
            Quests.Add(new QuestSample("BestScore!!!", "Get Over BestScore", ValueType.Coin, 400, 200));
            Quests.Add(new QuestSample("BestScore!!!!", "Get Over BestScore", ValueType.Coin, 800, 600));
            Quests.Add(new QuestSample("BestScore!!!!!", "Get Over BestScore", ValueType.Coin, 1600, 1000));
            Quests.Add(new QuestSample("BestScore!!!!!!", "Get Over BestScore", ValueType.Coin, 3200, 2500));
            Quests.Add(new QuestSample("BestScore!!!!!!!", "Get Over BestScore", ValueType.Coin, 6400, 5000));
            Quests.Add(new QuestSample("BestScore!!!!!!!!", "Get Over BestScore", ValueType.Coin, 12800, 8000));
            Quests.Add(new QuestSample("BestScore!!!!!!!!!", "Get Over BestScore", ValueType.Coin, 25600, 12000));
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

        if (removeSave) PlayerPrefs.DeleteKey(questSaveName);
    }

    string GetSaveData()
    {
        saveDataList.dataList = Quests.ToArray();

        string saveData = JsonUtility.ToJson(saveDataList, true);
        Debug.Log(saveData);
        return saveData;
    }

    void SetSaveData(string saveData)
    {
        saveDataList = JsonUtility.FromJson<QuestDataSaveList>(saveData);
        Quests = saveDataList.dataList.ToList();
    }
}

[System.Serializable]
public class QuestDataSaveList
{
    public QuestData[] dataList;
}

[System.Serializable]
public abstract class QuestData
{
    public string questName;
    public string questDesc;
    public ValueType rewardType;
    public int rewardValue;
    public float maxProgress;

    public abstract float curProgress { get; }
    public bool isQuestClear => curProgress >= maxProgress;

    public QuestData(string name, string desc, ValueType type, int reward, float maxValue)
    {
        questName = name;
        questDesc = desc;
        rewardType = type;
        rewardValue = reward;
        maxProgress = maxValue;
    }
}


// sample
[System.Serializable]
public class QuestSample : QuestData
{
    public QuestSample (string name, string desc, ValueType type, int reward, float maxValue) : base(name, desc, type, reward, maxValue)
    {
    }

    public override float curProgress => GameManager.Instance.bestScore;
}