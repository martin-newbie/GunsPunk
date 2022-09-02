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

            if (curHold >= minHold) anim.SetBool("IsLoad", true);
        }

    }

    public override void OnAttackEnd()
    {

        if (isHold && curHold >= maxHold)
        {
            anim.SetTrigger("AttackTrigger");
            Bullet temp = FireBullet();
            temp.notDestroy = true;
        }
        else if (curHold >= minHold)
        {
            anim.SetTrigger("AttackTrigger");
            FireBullet();
        }
        else
        {
            LoadCancel();
        }

        curHold = 0f;
        isHold = false;
    }

    protected virtual void LoadCancel()
    {
        anim.SetTrigger("LoadCancel");
    }

    public override void OnAttackStart()
    {
        if (AmmoCount > 0)
            isHold = true;
    }

    protected override Bullet FireBullet()
    {
        Bullet _bullet = null;
        anim.SetBool("IsLoad", false);

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
