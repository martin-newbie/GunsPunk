using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [Header("Explosive Bullet")]
    public float radius;
    public GameObject explosion;
    public ParticleSystem smokeEffect;

    protected override void OnBecameInvisible()
    {
        RemoveSmoke();
        base.OnBecameInvisible();
    }

    void RemoveSmoke()
    {
        smokeEffect.Stop();
        smokeEffect.transform.SetParent(null);
        smokeEffect.transform.localScale = Vector3.one;
        Destroy(smokeEffect.gameObject, 2f);
    }

    protected override void AttackAction(Collider2D collision)
    {
        base.AttackAction(collision);
        AudioManager.Instance.PlayEffectSound("Grenade");
        Instantiate(explosion, transform.position, Quaternion.identity);

        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Hostile"));
        RemoveSmoke();
        
        foreach (var item in objs)
        {
            Entity temp;
            if(item.TryGetComponent(out temp))
            {
                temp.OnHit(damage, transform);
            }
        }
    }

}
