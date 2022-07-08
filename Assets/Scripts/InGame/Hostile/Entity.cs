using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float maxHP;
    public float HP;
    public Vector3 HP_Gauge_Offset;
    protected Action OnHitAction;
    protected GaugeContainer hpGauge;

    private void Awake()
    {
        maxHP = HP;
        hpGauge = InGameManager.Instance.SpawnGaugeBar();
    }

    private void Update()
    {
        hpGauge.SetGauge(HP, maxHP);
        hpGauge.FollowTarget(HP_Gauge_Offset, transform);
    }

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
