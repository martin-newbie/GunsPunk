using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : Entity
{
    protected override void Awake()
    {
        base.Awake();
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
    }

    private void Update()
    {
        moveSpeed = InGameManager.Instance.objectSpeed;
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    void OnDie()
    {
        Destroy(gameObject);
    }
}