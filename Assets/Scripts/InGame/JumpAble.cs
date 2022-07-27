using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpAble : Entity
{
    [Header("JumpAble")]
    protected Collider2D bodyCol;
    protected Rigidbody2D RB;

    public bool isActing;
    protected bool goUpTrigger;
    protected bool goDownTrigger;

    [Header("Jump Value")]
    public float JumpForce = 13.5f;
    public float CheckRadius = 0.05f;
    public int curPosIdx;

    [Header("Transform")]
    public Transform FeetPos;
    public Transform TopPos;

    protected override void Awake()
    {
        base.Awake();
        bodyCol = GetComponent<Collider2D>();
        RB = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        GoingUpAction();
        GoingDownAction();
    }

    protected virtual void Update()
    {

    }


    protected bool checkFeet => Physics2D.OverlapCircle(FeetPos.position, CheckRadius, LayerMask.GetMask("Floor"));
    protected bool checkTop => Physics2D.OverlapCircle(TopPos.position, CheckRadius, LayerMask.GetMask("Floor"));

    enum JumpState
    {
        None,
        GoUp,
        GoDown,
        End
    }

    JumpState moveState;

    void GoingUpAction()
    {
        if (goUpTrigger && isActing)
        {

            switch (moveState)
            {
                case JumpState.None:
                    if (RB.velocity.y > 0f)
                    {
                        moveState = JumpState.GoUp;
                        bodyCol.isTrigger = true;
                    }
                    break;
                case JumpState.GoUp:
                    if (RB.velocity.y <= 0f)
                    {
                        moveState = JumpState.GoDown;
                        bodyCol.isTrigger = false;
                    }
                    break;
                case JumpState.GoDown:
                    if (RB.velocity == Vector2.zero && checkFeet)
                    {
                        moveState = JumpState.End;
                    }
                    break;
                case JumpState.End:
                    moveState = JumpState.None;
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
                case JumpState.None:
                    if (RB.velocity.y > 0f)
                    {
                        moveState = JumpState.GoUp;
                        bodyCol.isTrigger = true;
                    }
                    break;
                case JumpState.GoUp:
                    if (RB.velocity.y < 0f)
                    {
                        moveState = JumpState.GoDown;
                    }
                    break;
                case JumpState.GoDown:
                    if (checkTop)
                    {
                        bodyCol.isTrigger = false;
                        moveState = JumpState.End;
                    }
                    break;
                case JumpState.End:
                    if (checkFeet && RB.velocity == Vector2.zero)
                    {
                        moveState = JumpState.None;
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
            GoUpJump();
        }
    }

    protected virtual void GoUpJump()
    {
        isActing = true;
        goUpTrigger = true;

        RB.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
    }

    public void GoDown()
    {
        if (curPosIdx > 0 && !isActing)
        {
            GoDownJump();
        }
    }

    protected virtual void GoDownJump()
    {
        isActing = true;
        goDownTrigger = true;

        RB.AddForce(new Vector2(0, JumpForce / 3), ForceMode2D.Impulse);
    }

}
