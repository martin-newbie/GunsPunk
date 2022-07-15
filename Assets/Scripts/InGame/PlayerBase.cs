using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : JumpAble
{
    [Header("Status Value")]
    public float speed;
    public float fireRate;
    protected float fireDelay => 1f / (fireRate / 60f);
    protected float curDelay;
    public bool isAlive = true;

    [Header("Gun Value")]
    public float spread_pos;
    public float spread_rot;


    [Header("Gauge Value")]
    public float maxFever = 100f;
    public float feverIncrease;
    public float feverValue;
    protected bool isSkillActive;

    public int AmmoCount;
    public int MaxAmmo;

    [Header("Item Value")]
    public int AmmoIncrease;
    public float HealthIncrease;

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
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;

        AmmoCount = MaxAmmo;
    }

    public void UIInit()
    {
        InGameUIManager.Instance.SetPlayerHp(0f, maxHP);
        InGameUIManager.Instance.SetFeverGauge(0f, maxFever);
        InGameUIManager.Instance.SetAmmoGauge(0f, MaxAmmo);
    }

    protected override void Update()
    {
        if (!isAlive)
        {
            bodyCol.enabled = true;
            return;
        }

        if (InGameManager.Instance.isGameActive)
        {
            base.Update();
            SetGaugeUI();
        }
    }

    public void MoveForward(int dir = 1)
    {
        // state: move
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime * dir);
    }

    public void GetFever()
    {
        if (isSkillActive) return;

        feverValue += feverIncrease;
        if (feverValue >= maxFever)
        {
            isSkillActive = true;
            feverValue = 0f;
            Skill();
        }
    }

    void SetGaugeUI()
    {
        InGameUIManager.Instance.SetPlayerHp(HP, maxHP);
        InGameUIManager.Instance.SetFeverGauge(feverValue, maxFever);
        InGameUIManager.Instance.SetAmmoGauge(AmmoCount, MaxAmmo);
    }

    public override void OnHit(float damage, Transform hit)
    {
        if (InGameManager.Instance.isGameActive)
        {
            base.OnHit(damage, hit);
        }
    }

    void OnDie()
    {
        // gameover
        if (InGameManager.Instance.isGameActive)
        {
            isAlive = false;
            InGameManager.Instance.GameOver();
        }
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
            _bullet = Instantiate(bullet, FirePos.position + new Vector3(0, Random.Range(-spread_pos, spread_pos)), Quaternion.Euler(0, 0, Random.Range(-spread_rot, spread_rot)));
            _bullet.Init(speed, damage, this);
            AmmoCount--;

            InGameManager.Instance.SetRoundCoin(1);
        }

        return _bullet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            switch (collision.GetComponent<Item>().Case)
            {
                case ItemCase.Ammunition:
                    ItemAmmunition();
                    break;
                case ItemCase.Health:
                    ItemHealth();
                    break;
            }

            Destroy(collision.gameObject);
        }
    }

    public virtual void ItemHealth(float count = -1)
    {
        if (count == -1)
            HP += HealthIncrease;
        else HP += count;


        if (HP > maxHP) HP = maxHP;
    }

    public virtual void ItemAmmunition(int count = -1)
    {
        if (count == -1)
            AmmoCount += AmmoIncrease;
        else AmmoCount += count;

        if (AmmoCount > MaxAmmo) AmmoCount = MaxAmmo;
    }

    protected abstract void Skill();

    public abstract void OnAttackStart();
    public abstract void OnAttackEnd();
}
