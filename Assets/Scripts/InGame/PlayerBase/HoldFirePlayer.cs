using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldFirePlayer : PlayerBase
{
    [Header("Hold Fire Player")]
    public float maxHold = 5f;
    public float curHold;
    public bool isHold;

    protected override void Update()
    {
        base.Update();


        if (isHold)
        {
            if (curHold <= maxHold)
                curHold += Time.deltaTime;
            else
            {
                curHold = 0f;
                FireBullet();
            }
        }
    }

    public override void OnAttackEnd()
    {
        FireBullet();
        curHold = 0f;
        isHold = false;
    }

    public override void OnAttackStart()
    {
        isHold = true;
    }

    void FireBullet()
    {
        Bullet _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
        _bullet.Init(speed * (curHold / maxHold), damage * (curHold / maxHold));
    }
}
