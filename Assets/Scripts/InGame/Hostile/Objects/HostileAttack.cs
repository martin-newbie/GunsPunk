using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HostileAttack : MonoBehaviour
{
    public Vector2 size;
    public float damage;
    public bool isHitAble = true;
    public bool isTrigger = false;
    Action hitAction;

    public void Init(float _damage, Action hit = null)
    {
        damage = _damage;
        hitAction = hit;
    }

    private void Update()
    {
        if (!isTrigger && isHitAble)
        {
            var obj = Physics2D.OverlapBox(transform.position, size, 0f, LayerMask.GetMask("Player"));
            if (obj != null)
            {
                PlayerBase player = obj.GetComponent<PlayerBase>();
                player.OnHit(damage, transform);
                hitAction?.Invoke();
                isHitAble = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger && isHitAble)
        {
            if(collision.CompareTag("Player"))
            {
                PlayerBase player = collision.GetComponent<PlayerBase>();
                player.OnHit(damage, transform);
                isHitAble = false;
                hitAction?.Invoke();
            }
        }
    }
}
