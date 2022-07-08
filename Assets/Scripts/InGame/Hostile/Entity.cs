using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float HP;

    protected Action OnHitAction;

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Bullet temp = collision.GetComponent<Bullet>();

            HP -= temp.damage;
            if(HP <= 0)
            {
                OnHitAction?.Invoke();
            }
        }
    }
}
