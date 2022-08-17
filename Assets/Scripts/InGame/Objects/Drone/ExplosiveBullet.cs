using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [Header("Explosive Bullet")]
    public float radius;
    public GameObject explosion;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    protected override void AttackAction(Collider2D collision)
    {
        base.AttackAction(collision);
        AudioManager.Instance.PlayEffectSound("Grenade");
        Instantiate(explosion, transform.position, Quaternion.identity);

        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Hostile"));
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
