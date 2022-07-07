using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFirePlayer : PlayerBase
{

    public float fireRate;
    float fireDelay;
    float curDelay;
    bool isFire;

    protected override void Start()
    {
        base.Start();

        fireDelay = 1f / (fireRate / 60f);
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
        curDelay = 0f;
    }

    public override void OnAttackStart()
    {
        isFire = true;
    }
}
