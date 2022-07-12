using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HostileAttack : MonoBehaviour
{

    public float damage;
    Action hitAction;

    public void Init(float _damage, Action hit = null)
    {
        damage = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerBase player = collision.GetComponent<PlayerBase>();

            player.OnHit(damage);
            hitAction?.Invoke();
        }
    }
}
