using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGun : Monster
{
    [Header("Gun Monster")]
    public float standbyDelay;
    public float moveTime;
    public float startPosX;
    public Transform AttackPos;
    public Bullet EnemyBullet;

    protected override void Update()
    {
        base.Update();

        if (!nowActing)
        {
            switch (state)
            {
                case MonsterState.Appear:
                    AppearFunction();
                    break;
                case MonsterState.Standby:
                    StartCoroutine(StandbyCoroutine());
                    nowActing = true;
                    break;
                case MonsterState.Attack:

                    nowActing = true;
                    break;
                case MonsterState.Moving:
                    StartCoroutine(MovingCoroutine());
                    nowActing = true;
                    break;
            }
        }
    }

    void AppearFunction()
    {
        // state: moving
        if (transform.position.x > startPosX)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        else
        {
            state = MonsterState.Standby;
        }
    }

    IEnumerator StandbyCoroutine()
    {
        // state: standby
        yield return new WaitForSeconds(standbyDelay);
        state = MonsterState.Moving;

        nowActing = false;
        yield break;
    }

    IEnumerator MovingCoroutine()
    {
        yield return null;

        float timer = moveTime + Random.Range(-0.5f, 0.5f);
        int dir = transform.position.x <= 5.5f ? 1 : -1;
        // state: -1(move), 1(slow move)

        do
        {
            transform.Translate(Vector3.left * dir * moveSpeed * Time.deltaTime);

            if (transform.position.x > 9.5f) break;
            if (transform.position.x < 1.5f) break;

            timer -= Time.deltaTime;
            yield return null;
        } while (timer > 0);

        // state: standby

        yield return new WaitForSeconds(1f);
        state = MonsterState.Attack;

        nowActing = false;
        yield break;
    }

    IEnumerator AttackCoroutine()
    {
        switch (curPosIdx == InGameManager.Instance.CurPlayer.curPosIdx)
        {
            case true:
                yield return StartCoroutine(StraightAttack());
                break;


            case false:
                yield return StartCoroutine(DirectionAttack());
                break;
        }

        nowActing = false;
        yield break;
    }

    IEnumerator StraightAttack()
    {
        AttackPos.rotation = Quaternion.Euler(0, -180, 0);

        for (int i = 0; i < 5; i++)
        {
            
        }


        yield break;
    }

    IEnumerator DirectionAttack()
    {

        yield break;
    }
}
