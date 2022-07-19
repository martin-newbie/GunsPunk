using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3_KoreanArcher : HoldFirePlayer
{
    [Header("Korean Archer")]
    public Bullet CommonArrow;
    public Bullet ChargedArrow;
    public int coinCount;

    protected override void Skill()
    {
        for (int i = -45; i <= 45; i += 15)
        {
            Bullet temp = FireBullet();
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        isSkillActive = false;
    }

    protected override Bullet FireBullet()
    {
        Bullet _bullet;

        bullet = curHold >= maxHold ? ChargedArrow : CommonArrow;

        _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
        _bullet.Init(speed / 2f + speed * (curHold / maxHold), damage / 2 + damage * (curHold / maxHold), this, ()=> { InGameManager.Instance.GetRoundCoin(coinCount); });

        return _bullet;
    }
}
