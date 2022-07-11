using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity")]
    public float maxHP;
    public float HP;
    public float damage;
    protected float moveSpeed;

    protected Action<float> OnHitAction;
    protected Action OnDestroyAction;

    protected virtual void Awake()
    {
        HP = maxHP;
    }

    public virtual void OnHit(float damage)
    {
        HP -= damage;

        if(HP <= 0)
        {
            OnDestroyAction?.Invoke();
        }
    }

}
