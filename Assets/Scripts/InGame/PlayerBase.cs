using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : JumpAble
{
    [Header("Value")]
    public float speed;
    public float fireRate;
    public float maxFever = 100f;
    public float feverIncrease;
    public float feverValue;
    protected float fireDelay => 1f / (fireRate / 60f);
    protected float curDelay;
    public int AmmoCount;
    public int MaxAmmo;

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
        InGameUIManager.Instance.SetAmmoGauge(0f, MaxAmmo);

        OnHitAction = OnHit;
        OnDestroyAction = OnDie;

        transform.position = new Vector2(-4.5f, -2.72f);
        AmmoCount = MaxAmmo;
    }

    protected override void Update()
    {
        base.Update();
        SetGaugeUI();
    }

    public void GetFever()
    {
        feverValue += feverIncrease;
    }

    void SetGaugeUI()
    {
        InGameUIManager.Instance.SetPlayerHp(HP, maxHP);
        InGameUIManager.Instance.SetFeverGauge(feverValue, maxFever);
        InGameUIManager.Instance.SetAmmoGauge(AmmoCount, MaxAmmo);
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
        Bullet _bullet = null;

        if (AmmoCount > 0)
        {
            _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
            _bullet.Init(speed, damage, this);
            AmmoCount--;
        }

        return _bullet;
    }

    public abstract void OnAttackStart();
    public abstract void OnAttackEnd();
}
