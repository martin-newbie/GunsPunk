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
                Bullet temp = FireBullet();
                temp.notDestroy = true;
                curHold = 0f;
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

    Bullet FireBullet()
    {
        Bullet _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
        _bullet.Init(speed / 2f + speed * (curHold / maxHold), damage / 2 + damage * (curHold / maxHold));

        return _bullet;
    }
}
