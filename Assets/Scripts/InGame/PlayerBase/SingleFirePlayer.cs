using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleFirePlayer : PlayerBase
{
    [Header("Single Fire Player")]
    bool isFire;

    protected override void Update()
    {
        base.Update();

    }

    public override void OnAttackEnd()
    {
    }

    public override void OnAttackStart()
    {
        if (!isFire) StartCoroutine(SingleFireCoroutine());
    }

    IEnumerator SingleFireCoroutine()
    {
        isFire = true;
        FireBullet();
        yield return new WaitForSeconds(fireDelay);
        isFire = false;
    }
}
