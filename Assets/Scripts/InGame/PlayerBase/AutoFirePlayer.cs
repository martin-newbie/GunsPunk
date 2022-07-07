using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFirePlayer : PlayerBase
{
    [Header("Auto Fire Player")]
    protected bool isFire;

    protected override void Start()
    {
        base.Start();

        curDelay = fireDelay;
    }

    protected override void Update()
    {
        base.Update();

        if (isFire)
        {
            if (curDelay >= fireDelay)
            {
                Bullet _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
                _bullet.Init(speed, damage);
                curDelay = 0f;
            }

            curDelay += Time.deltaTime;
        }
    }

    public override void OnAttackEnd()
    {
        isFire = false;
        curDelay = fireDelay;
    }

    public override void OnAttackStart()
    {
        isFire = true;
    }
}
