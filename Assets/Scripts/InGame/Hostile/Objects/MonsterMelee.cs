using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterMelee : Monster
{

    [Header("Monster Melee")]
    public float standbyDelay;
    public float posX_Attack;   // 공격할 때 이 x좌표까지 이동
    public float posX_Standby;  // 대기할 때 이 x좌표까지 이동
    public float posX_Start;
    public HostileAttack AtkCol;

    [Header("Monster Melee Value")]
    public float atkDelay;

    protected override void Awake()
    {
        base.Awake();
        AtkCol.Init(damage);
    }

    protected override void Update()
    {
        if (!isAlive) return;
        anim.SetBool("IsGround", checkFeet && !isActing);

        base.Update();

        if (!nowActing)
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
                    nowCoroutine = StartCoroutine(MoveLogicCoroutine());
                    nowActing = true;
                    break;
            }
    }

    public override void OnDie()
    {
        base.OnDie();
        Destroy(AtkCol.gameObject);
    }

    void AppearFunction()
    {
        // state: moving
        anim.SetBool("IsMove", false);
        if (transform.position.x > posX_Start)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        else
        {
            state = MonsterState.Standby;
            anim.SetBool("IsMove", true);
            anim.SetInteger("Dir", -1);
        }
    }

    IEnumerator StandbyCoroutine()
    {
        // state: standby
        anim.SetBool("IsMove", true);
        anim.SetInteger("Dir", -1);
        yield return new WaitForSeconds(standbyDelay);
        state = MonsterState.Moving;

        nowActing = false;
        yield break;
    }

    IEnumerator MoveLogicCoroutine()
    {
        //state: move
        int idx = player.curPosIdx;

        while (idx != curPosIdx)
        {
            if (idx > curPosIdx)
                GoUp();
            else if (idx < curPosIdx)
                GoDown();

            yield return new WaitForSeconds(1.5f);
        }

        anim.SetBool("IsMove", true);
        anim.SetInteger("Dir", 1);
        do
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            yield return null;
        } while (transform.position.x >= posX_Attack);

        anim.SetInteger("Dir", -1);
        nowActing = false;
        state = MonsterState.Attack;
        yield break;
    }

    IEnumerator AttackCoroutine()
    {
        //state: attack
        anim.SetBool("IsMove", true);
        anim.SetInteger("Dir", -1);

        anim.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.PlayEffectSound("ChainSaw", AtkCol.transform.position);

        yield return new WaitForSeconds(0.2f);
        AtkCol.Init(damage);
        AtkCol.isHitAble = true;
        yield return new WaitForSeconds(atkDelay);
        AtkCol.isHitAble = false;

        yield return new WaitForSeconds(Random.Range(1f, 3f));

        //state: move back
        float x = posX_Standby + Random.Range(-2f, 1.5f);
        do
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            yield return null;
        } while (transform.position.x < x);

        nowActing = false;
        state = MonsterState.Standby;
        yield break;
    }
}
