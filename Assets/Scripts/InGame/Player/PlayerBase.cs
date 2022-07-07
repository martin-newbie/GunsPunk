using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    [Header("Value")]
    public float JumpForce;
    public float CheckRadius;
    public int curPosIdx;

    [Header("Transform")]
    public Transform FeetPos;
    public Transform TopPos;

    [Header("Objects")]
    public GameObject Bullet;

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
    }

    protected virtual void Update()
    {
        GoingUpAction();
        GoingDownAction();
        PCInput();
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

    bool nowMoving;
    void GoingUpAction()
    {
        if (goUpTrigger && isActing)
        {
            if (!nowMoving && Physics2D.OverlapCircle(TopPos.position, CheckRadius, LayerMask.GetMask("Floor")))
            {
                bodyCol.enabled = false;
                nowMoving = true;
            }

            if (nowMoving && Physics2D.OverlapCircle(FeetPos.position, CheckRadius, LayerMask.GetMask("Floor")))
            {
                bodyCol.enabled = true;
                nowMoving = false;
            }

            if (!nowMoving && bodyCol.enabled && RB.velocity == Vector2.zero)
            {
                goUpTrigger = false;
                isActing = false;
            }
        }
    }

    bool nowDown;
    void GoingDownAction()
    {
        if (goDownTrigger && isActing)
        {
            if (!nowMoving && Physics2D.OverlapCircle(FeetPos.position, CheckRadius, LayerMask.GetMask("Floor")) && !nowDown)
            {
                bodyCol.enabled = false;
                nowMoving = true;
            }

            if (nowMoving && Physics2D.OverlapCircle(TopPos.position, CheckRadius, LayerMask.GetMask("Floor")))
            {
                bodyCol.enabled = true;
                nowMoving = false;
                nowDown = true;
            }

            if(!nowMoving && bodyCol.enabled && RB.velocity == Vector2.zero && nowDown)
            {
                goDownTrigger = false;
                isActing = false;
                nowDown = false;
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
            curPosIdx++;
        }
    }

    public void GoDown()
    {
        if (curPosIdx > 0 && !isActing)
        {
            isActing = true;
            goDownTrigger = true;

            //RB.AddForce(new Vector2(0, JumpForce / 3), ForceMode2D.Impulse);
            curPosIdx--;
        }
    }
    #endregion

    public abstract void OnAttackStart();
    public abstract void OnAttackEnd();
}
