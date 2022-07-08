using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float maxHP;
    public float HP;
    public float moveSpeed = 3f;
    public Vector2 HP_Gauge_Offset = new Vector2(0, -1.3f);
    protected Action OnHitAction;
    protected GaugeContainer hpGauge;

    private void Awake()
    {
        maxHP = HP;
        hpGauge = InGameManager.Instance.SpawnGaugeBar();
        hpGauge.SetColor(Color.black, Color.red);
    }

    private void Update()
    {
        hpGauge.SetGauge(HP, maxHP);
        hpGauge.FollowTarget(HP_Gauge_Offset, transform);
    }

    protected virtual void MoveLogic()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
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
