using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CameraResolution
{

    public static void SetCameraResolution()
    {
        Camera cam = Camera.main;
        Rect rt = cam.rect;

        float scale_h = ((float)Screen.width / Screen.height) / (18.5f / 9f);
        float scale_w = 1f / scale_h;

        if (scale_h < 1)
        {
            rt.height = scale_h;
            rt.y = (1f - scale_h) / 2f;
        }
        else
        {
            rt.width = scale_w;
            rt.x = (1f - scale_w) / 2f;
        }
        cam.rect = rt;
    }
}

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
        damage = info.Damage + (level * info.DamageIncrease);
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
    public bool clear;

    [Header("Path")]
    public string characterInfoPath = "Scriptable/CharacterInfo/";
    public string itemInfoPath = "Scriptable/ItemInfo/";
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
    public ItemInfo[] itemsInfo;

    [Header("Status")]
    public int coin;     // ���� ��ȭ
    public int energy;      // ���� ��ȭ

    [Header("Only for quest")]
    public float bestScore;               // �ְ���
    public int acquiredCoin;            // ���� �÷��� �� ���� ����
    public int killMonsterCnt;          // ���� �÷����� ���� ���� ��
    public int destroyedObjectCnt;      // ���� �÷����� �μ� ��ֹ� ��
    public int hitBulletCnt;            // ���� �÷����� �� �Ǵ� ��ֹ��� ���� �Ѿ��� ��
    public int gamePlayCnt;             // ���� �÷��� Ƚ��

    [Header("Upgrade Status")]
    public int hpLevel = 1;
    public int ammoLevel = 1;
    public int defLevel = 1;

    public int maxLevel = 10;


    public float hpIncrease = 0.15f;
    public float ammoIncrease = 0.15f;
    public float defIncrease = 0.05f;

    public float hpValue => 1 + (hpIncrease * (hpLevel - 1));
    public float ammoValue => 1 + (ammoIncrease * (ammoLevel - 1));
    public float defValue => defIncrease * (defLevel - 1);

    [Header("About Character")]
    public int[] TrainingCost = new int[5]
    {
        1500,
        3500,
        4500,
        9500,
        500, // as energy
    };


    protected void Awake()
    {
        CameraResolution.SetCameraResolution();
        DontDestroyOnLoad(gameObject);

        charactersPrefab = Resources.LoadAll<PlayerBase>(prefabPath);
        charactersInfo = Resources.LoadAll<CharacterInfo>(characterInfoPath);
        itemsInfo = Resources.LoadAll<ItemInfo>(itemInfoPath);

        LoadJson();
        FindSelectedCharacter();
    }

    public void ChangeCharacterPos()
    {
        int temp = mainPlayerIdx;
        mainPlayerIdx = subPlayerIdx;
        subPlayerIdx = temp;
    }

    public int GetUpgradeCost(int level)
    {
        return 300 * level;
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
    }

    public void SetSubCharacter(int idx)
    {
        charactersInfo[subPlayerIdx].isSelected = false;

        charactersInfo[idx].isSelected = true;
        subPlayerIdx = idx;
    }

    private void Update()
    {
        PopUpInput();
    }

    public void ClearPopupStack()
    {
        PopupStack.Clear();
        curPopupIdx = 0;
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
                if (SceneManager.GetActiveScene().name == "MainScene")
                {

                }
                else if (SceneManager.GetActiveScene().name == "ShopScene")
                {
                    LoadingSceneManager.LoadScene("MainScene");
                }
                else if (SceneManager.GetActiveScene().name == "InGameScene")
                {
                    if (InGameManager.Instance.isGameActive)
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

    public void SetBestScore(float score)
    {
        if (score > bestScore) bestScore = score;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        if (clear)
        {
            PlayerPrefs.DeleteKey("StatusSaveData");
            PlayerPrefs.DeleteKey("Tutorial");
            return;
        }
        Save();
    }

    void Save()
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
        else
        {
            int idx = 0;
            foreach (var item in charactersInfo)
            {
                item.idx = idx;
                item.level = 0;
                item.trainingLevel = 0;
                item.exp = 0;

                if (item.idx < 2) item.isUnlocked = true;
                else item.isUnlocked = false;
                idx++;
            }

            foreach (var item in itemsInfo)
            {
                item.count = 0;
            }
        }
    }

    public string SaveJson()
    {
        DataSave save = new DataSave();
        save.CharactersInfo.Clear();
        save.ItemsInfo.Clear();

        foreach (var item in charactersInfo)
        {
            CharacterSave temp = new CharacterSave(item.exp, item.level, item.trainingLevel, item.isUnlocked, item.isSelected);
            save.CharactersInfo.Add(temp);
        }

        foreach (var item in itemsInfo)
        {
            ItemSave temp = new ItemSave(item.count);
            save.ItemsInfo.Add(temp);
        }

        save.userName = userName;
        save.userLevel = userLevel;
        save.userExp = userExp;

        save.curCoin = coin;
        save.energy = energy;

        save.bestScore = bestScore;
        save.acquiredCoin = acquiredCoin;
        save.killMonsterCnt = killMonsterCnt;
        save.destroyedObjectCnt = destroyedObjectCnt;
        save.hitBulletCnt = hitBulletCnt;
        save.gamePlayCnt = gamePlayCnt;

        save.hpLevel = hpLevel;
        save.ammoLevel = ammoLevel;
        save.defLevel = defLevel;

        save.mainChar = mainPlayerIdx;
        save.subChar = subPlayerIdx;

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

        for (int i = 0; i < save.ItemsInfo.Count; i++)
        {
            itemsInfo[i].count = save.ItemsInfo[i].count;
        }

        userName = save.userName;
        userLevel = save.userLevel;
        userExp = save.userExp;

        coin = save.curCoin;
        energy = save.energy;

        bestScore = save.bestScore;
        acquiredCoin = save.acquiredCoin;
        killMonsterCnt = save.killMonsterCnt;
        destroyedObjectCnt = save.destroyedObjectCnt;
        hitBulletCnt = save.hitBulletCnt;
        gamePlayCnt = save.gamePlayCnt;

        hpLevel = save.hpLevel;
        ammoLevel = save.ammoLevel;
        defLevel = save.defLevel;

        mainPlayerIdx = save.mainChar;
        subPlayerIdx = save.subChar;
    }
}

[System.Serializable]
public class DataSave
{
    public List<CharacterSave> CharactersInfo = new List<CharacterSave>();
    public List<ItemSave> ItemsInfo = new List<ItemSave>();
    public string userName;
    public int userLevel;
    public float userExp;

    public int curCoin;
    public int energy;

    public float bestScore;
    public int acquiredCoin;
    public int killMonsterCnt;
    public int destroyedObjectCnt;
    public int hitBulletCnt;
    public int gamePlayCnt;

    public int hpLevel;
    public int ammoLevel;
    public int defLevel;

    public int mainChar = 0;
    public int subChar = 1;
}

[System.Serializable]
public class CharacterSave
{
    public float exp;
    public int level;
    public int trainingLevel;
    public bool isUnlocked;
    public bool isSelected;

    public CharacterSave(float _exp, int _level, int _trLevl, bool _isUnlocked, bool _isSelected)
    {
        exp = _exp;
        level = _level;
        trainingLevel = _trLevl;
        isUnlocked = _isUnlocked;
        isSelected = _isSelected;
    }
}

[System.Serializable]
public class ItemSave
{
    public int count;

    public ItemSave(int _count)
    {
        count = _count;
    }
}