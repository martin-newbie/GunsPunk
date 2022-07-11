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
    public GameObject AtkCol;

    [Header("Monster Melee Value")]
    public float atkDelay;

    protected override void Update()
    {
        base.Update();

        if (!nowActing)
            switch (state)
            {
                case MonsterState.Appear:
                    AppearFunction();
                    break;
                case MonsterState.Standby:
                    Debug.Log("standby start");
                    nowActing = true;
                    nowCoroutine = StartCoroutine(StandbyCoroutine());
                    break;
                case MonsterState.Attack:
                    Debug.Log("attack start");
                    nowActing = true;
                    nowCoroutine = StartCoroutine(AttackCoroutine());
                    break;
                case MonsterState.Moving:
                    Debug.Log("move start");
                    nowActing = true;
                    nowCoroutine = StartCoroutine(MoveLogicCoroutine());
                    break;
            }
    }

    void AppearFunction()
    {
        // state: moving
        if (transform.position.x > posX_Start)
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
        Debug.Log("standby end");
        yield break;
    }

    IEnumerator MoveLogicCoroutine()
    {
        Debug.Log("move start");
        //state: move
        int idx = player.curPosIdx;
        Debug.Log(idx);

        while (idx != curPosIdx)
        {
            if (idx > curPosIdx)
                GoUp();
            else if (idx < curPosIdx)
                GoDown();

            yield return new WaitForSeconds(2f);
        }

        do
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            yield return null;
        } while (transform.position.x >= posX_Attack);

        nowActing = false;
        state = MonsterState.Attack;
        yield break;
    }

    IEnumerator AttackCoroutine()
    {
        //state: attack
        AtkCol.SetActive(true);
        yield return new WaitForSeconds(atkDelay);
        AtkCol.SetActive(false);

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
