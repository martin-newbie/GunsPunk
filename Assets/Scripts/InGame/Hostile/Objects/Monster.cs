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
    // 앞으로 빠르게 움직이는 로직에스ㅓ는 앞으로 움직이는 애니메이션을 재생

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
