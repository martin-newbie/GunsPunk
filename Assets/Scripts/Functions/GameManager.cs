using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("Path")]
    public string scriptablePath = "Scriptable/CharacterInfo/";
    public string prefabPath = "Prefabs/Player/Characters/";

    [Header("Player")]
    public string playerName = "LeeEunChan";

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
    public int curCoin;     // ���� ��ȭ
    public int energy;      // ���� ��ȭ

    [Header("Only for quest")]
    public int bestScore;               // �ְ���
    public int acquiredCoin;            // ���� �÷��� �� ���� ����
    public int killMonsterCnt;          // ���� �÷����� ���� ���� ��
    public int destroyedObjectCnt;      // ���� �÷����� �μ� ��ֹ� ��
    public int hitBulletCnt;            // ���� �÷����� �� �Ǵ� ��ֹ��� ���� �Ѿ��� ��
    public int gamePlayCnt;             // ���� �÷��� Ƚ��

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

        if(level == null)
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
}
