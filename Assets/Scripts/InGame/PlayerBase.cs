using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    [Header("Value")]
    public float JumpForce = 13f;
    public float CheckRadius = 0.05f;
    public int curPosIdx;
    public float damage;
    public float speed;
    public float fireRate;
    public float maxHP = 100f;
    public float HP;
    public float maxFever = 100f;
    public float feverValue;
    protected float fireDelay => 1f / (fireRate / 60f);
    protected float curDelay;

    [Header("Transform")]
    public Transform FeetPos;
    public Transform TopPos;

    [Header("Objects")]
    public Transform FirePos;
    public Bullet bullet;

    bool isActing;
    bool goUpTrigger;
    bool goDownTrigger;

    Collider2D bodyCol;
    Rigidbody2D RB;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(FeetPos.position, CheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(TopPos.position, CheckRadius);
    }

    protected virtual void Start()
    {
        bodyCol = GetComponent<Collider2D>();
        RB = GetComponent<Rigidbody2D>();

        transform.position = new Vector2(-4.5f, -2.72f);

        HP = maxHP;
        feverValue = maxFever;

        InGameUIManager.Instance.SetPlayerHp(0f, maxHP);
        InGameUIManager.Instance.SetFeverGauge(0f, maxFever);
    }

    protected virtual void Update()
    {
        GoingUpAction();
        GoingDownAction();
        PCInput();

        SetGaugeUI();
    }

    #region AllPlayerSame
    void PCInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GoUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoDown();
        }
    }

    bool checkFeet => Physics2D.OverlapCircle(FeetPos.position, CheckRadius, LayerMask.GetMask("Floor"));
    bool checkTop => Physics2D.OverlapCircle(TopPos.position, CheckRadius, LayerMask.GetMask("Floor"));

    enum PlayerMoveState
    {
        None,
        GoUp,
        GoDown,
        End
    }

    PlayerMoveState moveState;

    void GoingUpAction()
    {
        if (goUpTrigger && isActing)
        {

            switch (moveState)
            {
                case PlayerMoveState.None:
                    if (RB.velocity.y > 0f)
                    {
                        moveState = PlayerMoveState.GoUp;
                        bodyCol.enabled = false;
                    }
                    break;
                case PlayerMoveState.GoUp:
                    if (RB.velocity.y <= 0f)
                    {
                        moveState = PlayerMoveState.GoDown;
                        bodyCol.enabled = true;
                    }
                    break;
                case PlayerMoveState.GoDown:
                    if (RB.velocity == Vector2.zero && checkFeet)
                    {
                        moveState = PlayerMoveState.End;
                    }
                    break;
                case PlayerMoveState.End:
                    moveState = PlayerMoveState.None;
                    goUpTrigger = false;
                    isActing = false;
                    curPosIdx++;
                    break;
            }
        }
    }

    void GoingDownAction()
    {
        if (goDownTrigger && isActing)
        {

            switch (moveState)
            {
                case PlayerMoveState.None:
                    if (RB.velocity.y > 0f)
                    {
                        moveState = PlayerMoveState.GoUp;
                        bodyCol.enabled = false;
                    }
                    break;
                case PlayerMoveState.GoUp:
                    if (RB.velocity.y < 0f)
                    {
                        moveState = PlayerMoveState.GoDown;
                    }
                    break;
                case PlayerMoveState.GoDown:
                    if (checkTop)
                    {
                        bodyCol.enabled = true;
                        moveState = PlayerMoveState.End;
                    }
                    break;
                case PlayerMoveState.End:
                    if (checkFeet && RB.velocity == Vector2.zero)
                    {
                        moveState = PlayerMoveState.None;
                        goDownTrigger = false;
                        isActing = false;
                        curPosIdx--;
                    }
                    break;
            }
        }
    }

    public void GoUp()
    {
        if (curPosIdx < 2 && !isActing)
        {
            isActing = true;
            goUpTrigger = true;

            RB.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }

    public void GoDown()
    {
        if (curPosIdx > 0 && !isActing)
        {
            isActing = true;
            goDownTrigger = true;

            RB.AddForce(new Vector2(0, JumpForce / 3), ForceMode2D.Impulse);
        }
    }

    void SetGaugeUI()
    {
        InGameUIManager.Instance.SetPlayerHp(HP, maxHP);
        InGameUIManager.Instance.SetFeverGauge(feverValue, maxFever);
    }

    #endregion

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hostile"))
        {
            HitAction(collision.GetComponent<Entity>());
        }
    }

    public virtual void HitAction(Entity hostile)
    {
        HP -= hostile.damage;

        if (HP <= 0)
        {
            // game over
        }
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
