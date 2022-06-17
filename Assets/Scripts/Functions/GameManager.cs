using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    public string playerName;

    [Header("Status")]
    public int curCoin;     // ���� ��ȭ
    public int energy;      // ���� ��ȭ
    public int level = 1;

    public float curExp;
    public float maxExp => ReturnMaxExp(); //�������� ����

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
