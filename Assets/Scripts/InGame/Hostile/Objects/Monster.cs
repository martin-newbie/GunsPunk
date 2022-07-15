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
    // 움직이는 애니메이션은 항상 뒤로 움직이게끔
    // 앞으로 움직이는 로직에서는 뒤로 움직이는 애니메이션을 천천히 재생
    // 앞으로 빠르게 움직이는 로직에서는 앞으로 움직이는 애니메이션을 재생

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
