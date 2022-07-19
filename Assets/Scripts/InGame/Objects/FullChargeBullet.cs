using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullChargeBullet : Bullet
{
    [Header("Full Charge")]
    public ParticleSystem FullChargeEffect;

    protected override void OnBecameInvisible()
    {
        FullChargeEffect.transform.SetParent(transform.parent);
        Destroy(FullChargeEffect.gameObject, 2f);
        base.OnBecameInvisible();
    }
}
