using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed, damage;
    public bool notDestroy;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void Init(float _speed, float _damage)
    {
        speed = _speed;
        damage = _damage;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
