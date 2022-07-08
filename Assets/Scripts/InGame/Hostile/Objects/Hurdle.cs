using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : Entity
{
    private void Awake()
    {
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
    }

    void OnHit()
    {

    }

    void OnDie()
    {
        Destroy(hpGauge.gameObject);
        Destroy(gameObject);
    }
}
