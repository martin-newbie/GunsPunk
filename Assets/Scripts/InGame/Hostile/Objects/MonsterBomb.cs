using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBomb : Monster
{

    [Header("Suicide Monster")]
    public Explosion attackExplosion;
    public float minX;
    public bool isArrived;

    private void Start()
    {
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

        // 

        yield break;
    }


    public override void OnDie()
    {
        base.OnDie();
        // spawn explosion
    }
}
