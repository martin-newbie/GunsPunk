using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HostileAttack))]
public class Explosion : MonoBehaviour
{
    Collider2D atkCol;
    HostileAttack atk;

    void Start()
    {
        atkCol = GetComponent<Collider2D>();
        atk = GetComponent<HostileAttack>();
        atkCol.enabled = false;
    }

    void AttackOn()
    {
        atkCol.enabled = true;
    }

    void AttackOff()
    {
        atkCol.enabled = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity;
        if (collision.TryGetComponent(out entity))
        {
            entity.OnHit(atk.damage);
        }
    }
}
