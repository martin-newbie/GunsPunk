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
        if (saveData == "None")
        {
            // init quest
            var data = CSVReader.Read("QuestData");
            Quests.Clear();
            Debug.Log(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                Debug.Log(data[i]["Name"]);
                Debug.Log(data[i]["Desc"]);
                Debug.Log(data[i]["RewardType"]);
                Debug.Log(data[i]["RewardValue"]);
                Debug.Log(data[i]["MaxProgress"]);
                Debug.Log(data[i]["QuestType"]);

                string name = (string)data[i]["Name"];
                string desc = (string)data[i]["Desc"];
                ValueType reType = (ValueType)int.Parse(data[i]["RewardType"].ToString());
                int reValue = int.Parse(data[i]["RewardValue"].ToString());
                float progress = float.Parse(data[i]["MaxProgress"].ToString());
                QuestType quType = (QuestType)int.Parse(data[i]["QuestType"].ToString());

                QuestData quest = new QuestData(name, desc, reType, reValue, progress, quType);
                Quests.Add(quest);
            }

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
    public bool isRewardAble = true;

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