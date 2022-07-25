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
    public int curCoin;     // 무료 재화
    public int energy;      // 유료 재화
    public int level = 1;

    public float curExp;
    public float maxExp => ReturnMaxExp(); //저장하지 않음

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
