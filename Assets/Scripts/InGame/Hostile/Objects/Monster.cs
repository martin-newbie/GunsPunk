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
    // ������ ������ �����̴� ���������� ������ �����̴� �ִϸ��̼��� ���

    [Header("Monster AI")]
    public MonsterState state;
    public bool nowActing = false;
    public bool isAlive = true;
    public Coroutine nowCoroutine;
    public PlayerBase player;

    private void Start()
    {
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
        player = InGameManager.Instance.CurPlayer;

        nowActing = false;
        state = MonsterState.Appear;
    }

    public void Init(int idx)
    {
        curPosIdx = idx;
        transform.position = InGameManager.Instance.SpawnPoses[curPosIdx].position;
    }

    public override void OnHit(float damage, Transform hit)
    {
        if (isAlive)
        { 
            base.OnHit(damage, hit);
        }
    }

    public void OnDie()
    {
        if (nowCoroutine != null)
            StopCoroutine(nowCoroutine);

        player.ItemAmmunition(player.MaxAmmo / 20);

        isAlive = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        GetComponent<Collider2D>().enabled = false;

        GetComponent<EnemyHp>().DestroyGauge(3f);
        Destroy(gameObject, 3f);
    }
}
