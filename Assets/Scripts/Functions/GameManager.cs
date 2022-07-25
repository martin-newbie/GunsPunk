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

    [Header("Character")]
    public int mainPlayerIdx = 2;
    public int subPlayerIdx = 1;
    public PlayerBase[] charactersPrefab;
    public CharacterInfo[] charactersInfo;

    [Header("Status")]
    public int curCoin;     // ���� ��ȭ
    public int energy;      // ���� ��ȭ
    public int level = 1;

    public float curExp;
    public float maxExp => ReturnMaxExp(); //�������� ����

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
    }

    void LateUpdate()
    {
        if (curExp >= maxExp)
        {
            curExp -= maxExp;
            level++;
        }
    }

    public characterInfo GetCharacterInfo(int idx)
    {
        return new characterInfo(charactersInfo[idx]);
    }

    float ReturnMaxExp()
    {
        float ret;
        ret = (level + 2) * level * 50;

        return ret;
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
}
