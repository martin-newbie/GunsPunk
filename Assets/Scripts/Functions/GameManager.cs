using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    public string playerName;

    [Header("Character")]
    public int PlayerIdx = 0;
    public List<PlayerBase> PlayerPrefabs = new List<PlayerBase>();

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

    void LateUpdate()
    {
        if(curExp >= maxExp)
        {
            curExp -= maxExp;
            level++;
        }
    }

    float ReturnMaxExp()
    {
        float ret;
        ret = (level + 2) * level * 50;

        return ret;
    }
}
