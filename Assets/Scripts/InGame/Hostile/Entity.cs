using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity")]
    public float maxHP;
    public float HP;
    public float damage;
    public Vector2 HP_Gauge_Offset = new Vector2(0, -1.3f);
    protected float moveSpeed;
    protected GaugeContainer hpGauge;

    protected Action OnHitAction;
    protected Action OnDestroyAction;

    protected virtual void Start()
    {
        maxHP = HP;
        hpGauge = InGameManager.Instance.SpawnGaugeBar();
        hpGauge.SetColor(Color.black, Color.red);
    }

    protected virtual void Update()
    {
        moveSpeed = InGameManager.Instance.objectSpeed;

        hpGauge.SetGauge(HP, maxHP);
        hpGauge.FollowTarget(HP_Gauge_Offset, transform);

        MoveLogic();
    }

    protected virtual void MoveLogic()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet") && HP > 0)
        {
            Bullet temp = collision.GetComponent<Bullet>();
            temp.OnHostileHit();
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
