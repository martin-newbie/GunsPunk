using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : JumpAble
{
    [Header("Value")]
    public float speed;
    public float fireRate;
    public float maxFever = 100f;
    public float feverValue;
    protected float fireDelay => 1f / (fireRate / 60f);
    protected float curDelay;

    [Header("Objects")]
    public Transform FirePos;
    public Bullet bullet;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(FeetPos.position, CheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(TopPos.position, CheckRadius);
    }

    protected virtual void Start()
    {
        InGameUIManager.Instance.SetPlayerHp(0f, maxHP);
        InGameUIManager.Instance.SetFeverGauge(0f, maxFever);

        OnHitAction = OnHit;
        OnDestroyAction = OnDie;

        transform.position = new Vector2(-4.5f, -2.72f);

        feverValue = maxFever;
    }

    protected override void Update()
    {
        base.Update();
        SetGaugeUI();
    }

    void SetGaugeUI()
    {
        InGameUIManager.Instance.SetPlayerHp(HP, maxHP);
        InGameUIManager.Instance.SetFeverGauge(feverValue, maxFever);
    }

    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }

    void OnDie()
    {
        // gameover
        InGameManager.Instance.GameOver();
    }

    public virtual void AttackAction()
    {
        feverValue += 1f;
    }

    protected virtual Bullet FireBullet()
    {
        Bullet _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
        _bullet.Init(speed, damage, this);

        return _bullet;
    }

    public abstract void OnAttackStart();
    public abstract void OnAttackEnd();
}
