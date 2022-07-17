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

    GaugeContainer gauge;

    protected override void Start()
    {
        base.Start();
        gauge = InGameManager.Instance.SpawnGaugeBar(transform.position);
        gauge.SetColor(Color.gray, Color.white);
    }

    protected override void Update()
    {
        base.Update();


        if (isHold)
        {
            if (curHold < maxHold)
                curHold += Time.deltaTime;
        }

        gauge.SetGauge(curHold, maxHold);
        gauge.FollowTarget(new Vector2(0, 1.3f), transform);
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
            AmmoCount--;
        }

        return _bullet;
    }

    public override void ItemAmmunition(int count = -1)
    {
        InGameManager.Instance.roundCoin += coinValue;
    }
}
