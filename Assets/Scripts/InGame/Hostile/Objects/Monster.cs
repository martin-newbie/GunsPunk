using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Appear,
    Standby,
    Attack,
    Moving
}

public abstract class Monster : JumpAble
{
    // �����̴� �ִϸ��̼��� �׻� �ڷ� �����̰Բ�
    // ������ �����̴� ���������� �ڷ� �����̴� �ִϸ��̼��� õõ�� ���
    // ������ ������ �����̴� ���������ô� ������ �����̴� �ִϸ��̼��� ���

    [Header("Monster AI")]
    public MonsterState state;
    public bool nowActing = false;
    public Coroutine nowCoroutine;
    public PlayerBase player;

    private void Start()
    {
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
        player = InGameManager.Instance.CurPlayer;

        nowActing = false;
        state = MonsterState.Appear;

        Init(0);
    }

    public void Init(int idx)
    {
        curPosIdx = idx;
        transform.position = InGameManager.Instance.SpawnPoses[curPosIdx].position;
    }

    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }

    public void OnDie()
    {
        StopCoroutine(nowCoroutine);
    }
}
