using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed, damage;
    public bool notDestroy;
    PlayerBase player;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void Init(float _speed, float _damage, PlayerBase _player)
    {
        speed = _speed;
        damage = _damage;
        player = _player;
    }

    public void OnHostileHit()
    {
        player.AttackAction();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
