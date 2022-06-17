using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    public string playerName;

    [Header("Status")]
    public int curCoin;     // 무료 재화
    public int energy;      // 유료 재화
    public int level = 1;

    public float curExp;
    public float maxExp => ReturnMaxExp(); //저장하지 않음

    [Header("Only for quest")]
    public int bestScore;
    public int acquiredCoin;
    public int killMonsterCnt;
    public int destroyedObjectCnt;
    public int hitBulletCnt;
    public int gamePlayCnt;

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
