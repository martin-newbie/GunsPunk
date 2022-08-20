using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drone : MonoBehaviour
{
    [Header("Drone")]
    public Transform shootPos;
    public Vector2 offset = new Vector2(-1f, 1f);
    public float shootDelay;
    public float speed;

    protected Animator anim;
    protected float delay;
    protected PlayerBase player;

    [HideInInspector]public Transform target;
    [HideInInspector] public bool active;

    public void Init(Transform _target, PlayerBase _player)
    {
        target = _target;
        player = _player;
        active = true;

        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        Vector2 pos = Vector2.Lerp(transform.position, (Vector2)target.position + offset, Time.deltaTime * speed);
        transform.position = pos;

        if (!active) return;

        DelayLogic();
    }

    protected virtual void DelayLogic()
    {
        delay += Time.deltaTime;
        if (delay >= shootDelay)
        {
            Shoot();
            delay = 0f;
        }
    }

    protected virtual void Shoot()
    {
        anim.SetTrigger("AttackTrigger");
        Attack();
    }

    protected abstract Bullet Attack();
}
