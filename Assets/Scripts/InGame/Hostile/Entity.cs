using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity")]
    public float maxHP;
    public float HP;
    public float moveSpeed = 3f;
    public float damage;
    public Vector2 HP_Gauge_Offset = new Vector2(0, -1.3f);
    protected GaugeContainer hpGauge;

    protected Action OnHitAction;
    protected Action OnDestroyAction;

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
            OnHitAction?.Invoke();

            HP -= temp.damage;

            if (!temp.notDestroy) Destroy(temp.gameObject);

            if(HP <= 0)
            {
                OnDestroyAction?.Invoke();
            }
        }
    }
}
