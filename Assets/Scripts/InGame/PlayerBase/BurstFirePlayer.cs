using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFirePlayer : PlayerBase
{
    [Header("Burst Fire Player")]
    public int shootCount;
    bool fireAble;

    protected override void Start()
    {
        base.Start();
        fireAble = true;
    }

    public override void OnAttackEnd()
    {
    }

    public override void OnAttackStart()
    {
        if (fireAble)
            StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        fireAble = false;
        for (int i = 0; i < shootCount; i++)
        {
            FireBullet();
            yield return new WaitForSeconds(fireDelay);
        }

        fireAble = true;
        yield break;
    }
}
