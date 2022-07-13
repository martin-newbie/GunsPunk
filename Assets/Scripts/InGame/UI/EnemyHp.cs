using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Entity))]
public class EnemyHp : MonoBehaviour
{
    Entity thisEntity;
    GaugeContainer gauge;

    public Vector2 gaugeOffset;

    void Start()
    {
        thisEntity = GetComponent<Entity>();
        gauge = InGameManager.Instance.SpawnGaugeBar();

        gauge.SetColor(Color.black, Color.red);
    }

    void Update()
    {
        gauge.FollowTarget(gaugeOffset, gauge.transform);
        gauge.SetGauge(thisEntity.HP, thisEntity.maxHP);
    }
}
