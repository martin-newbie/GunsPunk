using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGun : Monster
{
    [Header("Gun Monster")]
    public float standbyDelay;
    public float moveTime;
    public float startPosX;

    protected override void Update()
    {
        base.Update();

        if (nowActing)
        {
            switch (state)
            {
                case MonsterState.Appear:
                    AppearFunction();
                    break;
                case MonsterState.Standby:
                    StartCoroutine(StandbyCoroutine());
                    nowActing = false;
                    break;
                case MonsterState.Attack:

                    nowActing = false;
                    break;
                case MonsterState.Moving:

                    nowActing = false;
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
    }

    IEnumerator MovingCoroutine()
    {
        yield return null;

        float time = moveTime + Random.Range(0, 0.5f);

        do
        {

        } while (true);

    }

}
