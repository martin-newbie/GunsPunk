using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Appear,
    Standby,
    Attack,
    Moving
}

public abstract class Monster : JumpAble
{
    // 움직이는 애니메이션은 항상 뒤로 움직이게끔
    // 앞으로 움직이는 로직에서는 뒤로 움직이는 애니메이션을 천천히 재생
    // 앞으로 빠르게 움직이는 로직에서는 앞으로 움직이는 애니메이션을 재생

    [Header("Monster AI")]
    public MonsterState state;
    public bool nowActing = false;
    public bool isAlive = true;
    public Coroutine nowCoroutine;
    public PlayerBase player;

    [Header("Monseter Parts")]
    public GameObject Body;
    public Rigidbody2D[] Parts;

    protected Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
        player = InGameManager.Instance.CurPlayer;

        nowActing = false;
        state = MonsterState.Appear;
    }

    public void Init(int idx)
    {
        curPosIdx = idx;
        transform.position = InGameManager.Instance.SpawnPoses[curPosIdx].position;
    }

    public override void OnHit(float damage, Transform hit)
    {
        if (isAlive)
        { 
            base.OnHit(damage, hit);
        }
    }

    public void OnDie()
    {
        if (nowCoroutine != null)
            StopCoroutine(nowCoroutine);

        player.ItemAmmunition(player.MaxAmmo / 20);

        isAlive = false;
        GetComponent<EnemyHp>().DestroyGauge();
        Body.SetActive(false);
        RB.bodyType = RigidbodyType2D.Static;

        foreach (var item in Parts)
        {
            item.gameObject.SetActive(true);
            item.AddForce(new Vector2(Random.Range(-6, 6), Random.Range(4, 10)), ForceMode2D.Impulse);
            item.AddTorque(Random.Range(-5f, 5f));
            Destroy(item.gameObject, Random.Range(5, 15));
        }

        StartCoroutine(DestroyMove(-1));
    }

    IEnumerator DestroyMove(int dir = 1)
    {

        do
        {
            if (dir == 1 && transform.position.x > 13f) break;
            if (dir == -1 && transform.position.x < -13f) break;

            transform.Translate(Vector3.right * dir * InGameManager.Instance.objectSpeed * Time.deltaTime);

            yield return null;
        } while (true);

        Destroy(gameObject);
        yield break;
    }
}
