using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HoldFirePlayer : PlayerBase
{
    [Header("Hold Fire Player")]
    public float maxHold = 1.5f;
    public float minHold = 0.3f;
    public float curHold;
    public bool isHold;
    public int coinValue = 50;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        InGameUIManager.Instance.SetHoldGauge(curHold / maxHold);

        if (isHold)
        {
            if (curHold < maxHold)
                curHold += Time.deltaTime;
        }

    }

    public override void OnAttackEnd()
    {

        if (curHold >= maxHold)
        {
            Bullet temp = FireBullet();
            temp.notDestroy = true;
        }
        else if (curHold >= minHold)
            FireBullet();

        curHold = 0f;
        isHold = false;
    }

    public override void OnAttackStart()
    {
        isHold = true;
    }

    protected override Bullet FireBullet()
    {
        Bullet _bullet = null;

        if (AmmoCount > 0)
        {
            _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
            _bullet.Init(speed / 2f + speed * (curHold / maxHold), damage / 2 + damage * (curHold / maxHold), this);

            if (!isSkillActive)
                AmmoCount--;
        }

        return _bullet;
    }

}
