using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [Header("Granade")]
    public float duration = 5f;
    public float explosionRad = 3f;
    public GameObject explosion;

    float damage;
    Rigidbody2D RB;
    bool isAttackAble = true;
    Coroutine explosionCoroutine;

    public void Init(Vector2 force, float torque, float damage)
    {
        this.damage = damage;

        RB = GetComponent<Rigidbody2D>();

        RB.AddForce(force, ForceMode2D.Impulse);
        RB.AddTorque(torque);

        explosionCoroutine = StartCoroutine(ExplosionCoroutine(duration));
    }

    IEnumerator ExplosionCoroutine(float wait)
    {
        yield return new WaitForSeconds(wait);
        ExplosionFunc();
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hostile"))
        {
            if (explosionCoroutine != null) StopCoroutine(explosionCoroutine);
            ExplosionFunc();
        }
    }

    void ExplosionFunc()
    {
        if (isAttackAble)
        {
            var hostile = Physics2D.OverlapCircleAll(transform.position, explosionRad, LayerMask.GetMask("Hostile"));
            foreach (var item in hostile)
            {
                item.GetComponent<Entity>().OnHit(damage, transform);
            }

            Instantiate(explosion, transform.position, Quaternion.identity);
            isAttackAble = false;
        }

    }
}
