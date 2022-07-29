using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuster : Monster
{

    [Header("Suicide Monster")]
    public Explosion attackExplosion;
    public float minX;
    public bool isArrived;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(MoveLogicCoroutine());
    }

    protected override void Update()
    {
        if (!isAlive) return;
        base.Update();
    }

    IEnumerator MoveLogicCoroutine()
    {
        // move
        while (transform.position.x >= minX)
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            yield return null;
        }

        // attack
        Explosion temp = Instantiate(attackExplosion, transform.position + new Vector3(0, 1f), Quaternion.identity);
        temp.GetComponent<HostileAttack>().Init(damage);
        OnHit(maxHP, transform);
        yield break;
    }
}
