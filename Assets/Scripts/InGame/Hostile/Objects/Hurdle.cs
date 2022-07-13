using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : Entity
{
    public HostileAttack atkCol;

    protected override void Awake()
    {
        base.Awake();
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
        atkCol.Init(damage, OnDie);
    }

    private void Update()
    {
        moveSpeed = InGameManager.Instance.objectSpeed;
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < -12f) OnDie();
    }

    void OnDie()
    {

        if (Random.Range(0, 100) < 25)
            InGameManager.Instance.SpawnRandomItem(transform.position);

        Destroy(GetComponent<EnemyHp>().gauge.gameObject);
        Destroy(gameObject);
    }

}