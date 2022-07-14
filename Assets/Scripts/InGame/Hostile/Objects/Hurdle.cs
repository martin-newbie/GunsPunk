using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : Entity
{
    public HostileAttack atkCol;
    public float explosionChance;

    protected override void Awake()
    {
        base.Awake();
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
        atkCol.Init(damage, OnDie);
    }

    private void Update()
    {
        if (InGameManager.Instance.isGameActive)
        {
            moveSpeed = InGameManager.Instance.objectSpeed;
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (transform.position.x < -12f) OnDie();
    }

    void OnDie()
    {

        if (Random.Range(0, 100) < 25)
            InGameManager.Instance.SpawnRandomItem(transform.position);
        if(Random.Range(0f, 100f) < explosionChance)
        {
            Explosion();
        }

        Destroy(GetComponent<EnemyHp>().gauge.gameObject);
        Destroy(gameObject);
    }

    void Explosion()
    {

    }
}