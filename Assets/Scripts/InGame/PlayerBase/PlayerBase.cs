using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : JumpAble
{

    Animator anim;

    [Header("Status Value")]
    public float speed;
    public float fireRate;
    protected float fireDelay => 1f / (fireRate / 60f);
    protected float curDelay;

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
    public ParticleSystem GunFireEffect;
    public ParticleSystem BulletShellEffect;
    public Transform FirePos;
    public Bullet bullet;

    public void Init(characterInfo info)
    {
        maxHP = info.hp;
        HP = maxHP;
        damage = info.damage;
        fireRate = info.rpm;
        feverIncrease = info.fever;
        spread_pos = info.spreadPos;
        spread_rot = info.spreadRot;
        MaxAmmo = info.ammo;
        AmmoIncrease = info.ammoItem;
        HealthIncrease = info.hpItem;
        speed = info.bulletSpeed;
    }

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

        anim = GetComponent<Animator>();
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


        anim.SetBool("IsGround", checkFeet && !isActing);

        if (InGameManager.Instance.isGameActive)
        {
            base.Update();
            SetGaugeUI();

            ComputerDebug();

            SetJumpIndex();
        }
    }

    void SetJumpIndex()
    {
        if (transform.position.y > -4.5f && transform.position.y < -3f)
        {
            curPosIdx = 0;
        }
        else if (transform.position.y > -1.75f && transform.position.y < 0f)
        {
            curPosIdx = 1;
        }
        else if (transform.position.y > 1f && transform.position.y < 2f)
        {
            curPosIdx = 2;
        }
    }

    void ComputerDebug()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) GoUp();
        if (Input.GetKeyDown(KeyCode.DownArrow)) GoDown();

        if (Input.GetKeyDown(KeyCode.Space)) OnAttackStart();
        if (Input.GetKeyUp(KeyCode.Space)) OnAttackEnd();
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
            InGameManager.Instance.Cam.ShakeForTime();
            InGameUIManager.Instance.DamagedEffect();
        }
    }

    void OnDie()
    {
        // gameover
        if (InGameManager.Instance.isGameActive)
        {
            isAlive = false;
            anim.SetTrigger("DieTrigger");
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
            anim.SetTrigger("AttackTrigger");
            _bullet = Instantiate(bullet, FirePos.position + new Vector3(0, Random.Range(-spread_pos, spread_pos)), Quaternion.Euler(0, 0, Random.Range(-spread_rot, spread_rot)));
            _bullet.Init(speed, damage, this);
            if (!isSkillActive)
                AmmoCount--;
            BulletShellEffect.Play();
            InGameManager.Instance.GetRoundCoin(1);
            GunFireEffect?.Play();
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


        if (HP >= maxHP) HP = maxHP;
    }

    public virtual void ItemAmmunition(int count = -1)
    {

        if (count == -1)
        {
            count = AmmoIncrease;
        }

        AmmoCount += count;

        InGameManager.Instance.AmmoEffect(transform.position + Vector3.up, count);

        if (AmmoCount >= MaxAmmo) AmmoCount = MaxAmmo;
    }

    protected abstract void Skill();

    protected override void GoDownJump()
    {
        base.GoDownJump();
        anim.SetTrigger("JumpTrigger");
    }

    protected override void GoUpJump()
    {
        base.GoUpJump();
        anim.SetTrigger("JumpTrigger");
    }

    public abstract void OnAttackStart();
    public abstract void OnAttackEnd();
}
