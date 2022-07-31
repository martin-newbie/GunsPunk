using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct characterInfo
{
    public int level;
    public float hp;
    public int ammo;
    public float damage;
    public float fever;

    public float bulletSpeed;
    public float rpm;
    public float spreadPos;
    public float spreadRot;
    public int ammoItem;
    public float hpItem;

    public characterInfo(CharacterInfo info)
    {
        level = info.trainingLevel;
        hp = info.HP + (level * info.HPIncrease);
        ammo = info.Ammo + (level * info.AmmoIncrease);
        damage = info.Damage + (level * info.Damage);
        fever = info.Fever + (level * info.FeverIncrease);

        bulletSpeed = info.bulletSpeed;
        rpm = info.rpm;
        spreadPos = info.SpreadPos;
        spreadRot = info.SpreadRot;
        ammoItem = info.AmmoItemValue;
        hpItem = info.HPItemValue;
    }
}

public class GameManager : Singleton<GameManager>
{
    public Stack<IPopUp> PopupStack = new Stack<IPopUp>();
    public int curPopupIdx;

    [Header("Path")]
    public string scriptablePath = "Scriptable/CharacterInfo/";
    public string prefabPath = "Prefabs/Player/Characters/";

    [Header("Player")]
    public string userName = "LeeEunChan";

    [Header("User")]
    public int userLevel;
    public float userExp;
    public float userMaxExp => GetUserMaxExp();

    [Header("Character")]
    public int mainPlayerIdx;
    public int subPlayerIdx;
    public PlayerBase[] charactersPrefab;
    public CharacterInfo[] charactersInfo;

    [Header("Status")]
    public int curCoin;     // 무료 재화
    public int energy;      // 유료 재화

    [Header("Only for quest")]
    public int bestScore;               // 최고기록
    public int acquiredCoin;            // 게임 플레이 중 얻은 코인
    public int killMonsterCnt;          // 게임 플레이중 죽인 몬스터 수
    public int destroyedObjectCnt;      // 게임 플레이중 부순 장애물 수
    public int hitBulletCnt;            // 게임 플레이중 적 또는 장애물에 맞춘 총알의 수
    public int gamePlayCnt;             // 게임 플레이 횟수

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

        charactersPrefab = Resources.LoadAll<PlayerBase>(prefabPath);
        charactersInfo = Resources.LoadAll<CharacterInfo>(scriptablePath);

        FindSelectedCharacter();
    }

    public void SetUserExp(float _exp)
    {
        userExp += _exp;

        while (userExp >= userMaxExp)
        {
            userExp -= userMaxExp;
            userLevel++;
        }
    }

    public float GetUserMaxExp(int? level = null)
    {
        float exp;

        if (level == null)
        {
            exp = (userLevel + 2) * userLevel * 50f;
        }
        else
        {
            int _level = (int)level;
            exp = (_level + 2) * _level * 50f;
        }

        return exp;

    }

    void FindSelectedCharacter()
    {

        foreach (var item in charactersInfo)
        {
            item.isSelected = false;
        }

        mainPlayerIdx = PlayerPrefs.GetInt("MainIdx", 0);
        subPlayerIdx = PlayerPrefs.GetInt("SubIdx", 1);

        SetMainCharacter(mainPlayerIdx);
        SetSubCharacter(subPlayerIdx);
    }

    public CharacterInfo GetMainPlayer()
    {
        return charactersInfo[mainPlayerIdx];
    }

    public characterInfo GetCharacterInfo(int idx)
    {
        return new characterInfo(charactersInfo[idx]);
    }

    public void SetMainCharacter(int idx)
    {
        charactersInfo[mainPlayerIdx].isSelected = false;

        charactersInfo[idx].isSelected = true;
        mainPlayerIdx = idx;
        PlayerPrefs.SetInt("MainIdx", mainPlayerIdx);
    }

    public void SetSubCharacter(int idx)
    {
        charactersInfo[subPlayerIdx].isSelected = false;

        charactersInfo[idx].isSelected = true;
        subPlayerIdx = idx;
        PlayerPrefs.SetInt("SubIdx", subPlayerIdx);
    }

    private void Update()
    {
        PopUpInput();
    }

    public void ClearPopupStack()
    {
        PopupStack.Clear();
    }

    void PopUpInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (curPopupIdx > 0)
            {
                PopupClose();
            }
            else
            {
                if(SceneManager.GetActiveScene().name == "MainScene")
                {

                }
                else if(SceneManager.GetActiveScene().name == "ShopScene")
                {
                    LoadingSceneManager.LoadScene("MainScene");
                }
                else if(SceneManager.GetActiveScene().name == "InGameScene")
                {
                    InGameUIManager.Instance.PauseOn();
                }
            }
        }
    }

    public void AddPopup(IPopUp popup)
    {
        PopupStack.Push(popup);
        curPopupIdx++;
    }

    public void PopupClose()
    {
        IPopUp popup = PopupStack.Pop();
        popup.Close();
        curPopupIdx--;
    }


    private void OnApplicationPause(bool pause)
    {
        string save = SaveJson();
        // at server

        // at local
        PlayerPrefs.SetString("StatusSaveData", save);
    }

    void LoadJson()
    {
        string content = PlayerPrefs.GetString("StatusSaveData", "Default");
        if (content != "Default") SetJson(content);
    }

    public string SaveJson()
    {
        DataSave save = new DataSave();
        save.CharactersInfo.Clear();
        foreach (var item in charactersInfo)
        {
            save.CharactersInfo.Add(item);
        }

        save.userName = userName;
        save.userLevel = userLevel;
        save.userExp = userExp;

        string jsonSave = JsonUtility.ToJson(save, true);
        return jsonSave;
    }

    public void SetJson(string contents)
    {
        DataSave save = JsonUtility.FromJson<DataSave>(contents);

        if (save == null) return;

        for (int i = 0; i < save.CharactersInfo.Count; i++)
        {
            charactersInfo[i].level = save.CharactersInfo[i].level;
            charactersInfo[i].trainingLevel = save.CharactersInfo[i].trainingLevel;
            charactersInfo[i].exp = save.CharactersInfo[i].exp;
            charactersInfo[i].isUnlocked = save.CharactersInfo[i].isUnlocked;
            charactersInfo[i].isSelected = save.CharactersInfo[i].isSelected;
        }

        userName = save.userName;
        userLevel = save.userLevel;
        userExp = save.userExp;
    }
}

[System.Serializable]
public class DataSave
{
    public List<CharacterInfo> CharactersInfo = new List<CharacterInfo>();
    public string userName;
    public int userLevel;
    public float userExp;
}