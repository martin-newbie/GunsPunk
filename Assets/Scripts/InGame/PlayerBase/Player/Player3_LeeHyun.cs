using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3_LeeHyun : HoldFirePlayer
{
    [Header("Korean Archer")]
    public Bullet CommonArrow;
    public Bullet ChargedArrow;
    public int coinCount;
    AudioObject arrowLoading;
    protected override string fireSound => "ArrowShoot";

    protected override void Skill()
    {
        for (int i = -45; i <= 45; i += 15)
        {
            curHold = maxHold;
            Bullet temp = FireBullet();
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }
        curHold = 0f;
        isSkillActive = false;
    }

    protected override Bullet FireBullet()
    {
        Bullet _bullet = null;
        if (AmmoCount > 0)
        {
            Bullet bulletPrefab = curHold >= maxHold ? ChargedArrow : CommonArrow;

            _bullet = Instantiate(bulletPrefab, FirePos.position, Quaternion.identity);
            _bullet.Init(speed / 2f + speed * (curHold / maxHold), damage / 2 + damage * (curHold / maxHold), this, () => { InGameManager.Instance.GetRoundCoin(coinCount); });

            if (curHold >= maxHold) _bullet.notDestroy = true;

            if (!isSkillActive)
                AmmoCount--;
        }

        return _bullet;
    }

    public override void OnAttackStart()
    {
        base.OnAttackStart();

        if (isHold) anim.SetTrigger("LoadTrigger");

        if (AmmoCount > 0)
        {
            AudioManager.Instance.PlayEffectSound("ArrowLoad");
            arrowLoading = AudioManager.Instance.PlayEffectSound("ArrowLoading");
        }
    }

    public override void OnAttackEnd()
    {
        arrowLoading?.Audio.Stop();
        if (curHold >= minHold)
        {
            if (curHold >= maxHold)
                AudioManager.Instance.PlayEffectSound("ArrowPower");
            AudioManager.Instance.PlayEffectSound("ArrowShoot");
        }

        base.OnAttackEnd();
    }
}
