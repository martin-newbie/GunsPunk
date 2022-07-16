using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HostileAttack : MonoBehaviour
{
    public Vector2 size;
    public float damage;
    public bool isHitAble = true;
    Action hitAction;

    public void Init(float _damage, Action hit = null)
    {
        damage = _damage;
        hitAction = hit;
    }

    private void Update()
    {
        if (isHitAble)
        {
            var player = Physics2D.OverlapBox(transform.position, size, 0f, LayerMask.GetMask("Player")).GetComponent<PlayerBase>();
            if (player != null)
            {
                player.OnHit(damage, transform);
                hitAction?.Invoke();
                isHitAble = false;
            }
        }
    }
}
