using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public float speed, damage;
    public bool notDestroy;
    PlayerBase player;
    Action hitAction;

    protected virtual void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void Init(float _speed, float _damage, PlayerBase _player, Action hit = null)
    {
        speed = _speed;
        damage = _damage;
        player = _player;
        hitAction = hit;
    }

    public void OnHostileHit()
    {
        player.AttackAction();
    }

    protected virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hostile"))
        {
            AttackAction(collision);
        }
    }

    protected virtual void AttackAction(Collider2D collision)
    {
        Entity entity = collision.GetComponent<Entity>();
        if (entity != null && entity.isAlive)
        {
            GameManager.Instance.hitBulletCnt++;

            entity.OnHit(damage, transform);
            player.GetFever();
            hitAction?.Invoke();
            if (!notDestroy) Destroy(gameObject);
        }
    }
}
