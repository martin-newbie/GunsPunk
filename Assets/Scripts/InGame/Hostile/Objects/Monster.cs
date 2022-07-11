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
    public bool nowActing = true;
    public Coroutine nowCoroutine;

    private void Start()
    {
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
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
