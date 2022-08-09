using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public void Init(float _speed, float _damage)
    {
        speed = _speed;
        damage = _damage;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerBase player = collision.GetComponent<PlayerBase>();
            if (player.isAlive)
                player.OnHitAction?.Invoke(damage, transform);

            Destroy(gameObject);
        }
    }
}
