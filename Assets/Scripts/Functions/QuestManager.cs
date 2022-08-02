using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;


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
            Quests.Add(new QuestData("BestScore!", "Get Over BestScore", ValueType.Coin, 100, 50, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!", "Get Over BestScore", ValueType.Coin, 200, 100, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!!", "Get Over BestScore", ValueType.Coin, 400, 200, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!!!", "Get Over BestScore", ValueType.Coin, 800, 600, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!!!!", "Get Over BestScore", ValueType.Coin, 1600, 1000, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!!!!!", "Get Over BestScore", ValueType.Coin, 3200, 2500, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!!!!!!", "Get Over BestScore", ValueType.Coin, 6400, 5000, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!!!!!!!", "Get Over BestScore", ValueType.Coin, 12800, 8000, QuestType.BestScore));
            Quests.Add(new QuestData("BestScore!!!!!!!!!", "Get Over BestScore", ValueType.Coin, 25600, 12000, QuestType.BestScore));

            saveDataList.dataList = Quests.ToArray();
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

        string saveData = JsonConvert.SerializeObject(saveDataList);
        Debug.Log(saveData);
        return saveData;
    }

    void SetSaveData(string saveData)
    {
        saveDataList = JsonConvert.DeserializeObject<QuestDataSaveList>(saveData);
        Quests = saveDataList.dataList.ToList();
    }
}

[System.Serializable]
public enum QuestType
{
    UserLevel,
    BestScore,
    InGameCoin,
    MonsterKill,
    HurdelDestroy,
    BulletHit,
    GamePlayCount
}

[System.Serializable]
public class QuestDataSaveList
{
    public QuestData[] dataList;
}

[System.Serializable]
public class QuestData
{
    public string questName;
    public string questDesc;
    public ValueType rewardType;
    public QuestType questType;
    public int rewardValue;
    public float maxProgress;
    public float curProgress => questType switch
    {
        QuestType.UserLevel => GameManager.Instance.userLevel,
        QuestType.BestScore => GameManager.Instance.bestScore,
        QuestType.InGameCoin => GameManager.Instance.acquiredCoin,
        QuestType.MonsterKill => GameManager.Instance.killMonsterCnt,
        QuestType.HurdelDestroy => GameManager.Instance.destroyedObjectCnt,
        QuestType.BulletHit => GameManager.Instance.hitBulletCnt,
        QuestType.GamePlayCount => GameManager.Instance.gamePlayCnt,
        _ => throw new System.NotImplementedException(),
    };

    public bool isQuestClear => curProgress >= maxProgress;

    public QuestData(string name, string desc, ValueType value, int reward, float maxValue, QuestType quest)
    {
        questName = name;
        questDesc = desc;
        rewardType = value;
        rewardValue = reward;
        maxProgress = maxValue;
        questType = quest;
    }
}