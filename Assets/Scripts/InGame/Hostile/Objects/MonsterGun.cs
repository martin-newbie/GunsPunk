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
    public EnemyBullet bullet;

    [Header("Gun Monster Value")]
    public float fireDelay;
    public float b_speed;
    public float b_damage;

    protected override void Update()
    {
        if (!isAlive) return;

        base.Update();

        if (!nowActing)
        {
            switch (state)
            {
                case MonsterState.Appear:
                    AppearFunction();
                    break;
                case MonsterState.Standby:
                    nowCoroutine = StartCoroutine(StandbyCoroutine());
                    nowActing = true;
                    break;
                case MonsterState.Attack:
                    nowCoroutine = StartCoroutine(AttackCoroutine());
                    nowActing = true;
                    break;
                case MonsterState.Moving:
                    nowCoroutine = StartCoroutine(MovingCoroutine());
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

        if (player.curPosIdx > curPosIdx) GoUp();
        else if (player.curPosIdx < curPosIdx) GoDown();

        yield return new WaitForSeconds(2f);

        float timer = moveTime + Random.Range(-1.5f, 0.5f);
        int dir = transform.position.x >= 5.5f ? 1 : -1;
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
        switch (curPosIdx == player.curPosIdx)
        {
            case true:
                yield return StartCoroutine(StraightAttack());
                break;


            case false:
                yield return StartCoroutine(DirectionAttack());
                break;
        }

        //state: standby
        nowActing = false;
        state = MonsterState.Standby;
        yield break;
    }

    IEnumerator StraightAttack()
    {
        // state: straight attack

        AttackPos.rotation = Quaternion.Euler(0, -180, 0);

        for (int i = 0; i < 5; i++)
        {
            FireBullet(AttackPos.position, AttackPos.rotation);
            yield return new WaitForSeconds(fireDelay);
        }

        yield return new WaitForSeconds(fireDelay);
        // state: standby
        yield break;
    }

    IEnumerator DirectionAttack()
    {

        Vector3 playerPos = player.transform.position;
        if (playerPos.y > AttackPos.position.y) playerPos += new Vector3(1f, 1f);

        Vector3 dir = playerPos - AttackPos.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        AttackPos.rotation = rot;

        for (int i = 0; i < 3; i++)
        {
            FireBullet(AttackPos.position, AttackPos.rotation);
            yield return new WaitForSeconds(fireDelay);
        }

        yield return new WaitForSeconds(fireDelay);
        // state: standby
        yield break;
    }

    EnemyBullet FireBullet(Vector3 pos, Quaternion rot)
    {
        EnemyBullet temp = Instantiate(bullet, pos, rot);
        temp.Init(b_speed, b_damage);
        return temp;
    }
}
