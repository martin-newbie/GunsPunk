using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Grenade")]
    public float duration = 5f;
    public float explosionRad = 3f;
    public GameObject explosion;

    float damage;
    Rigidbody2D RB;
    bool isAttackAble = true;
    Coroutine explosionCoroutine;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRad);
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hostile"))
        {
            if (explosionCoroutine != null) StopCoroutine(explosionCoroutine);
            ExplosionFunc();
        }
    }

    void ExplosionFunc()
    {
        if (isAttackAble)
        {
            AudioManager.Instance.PlayEffectSound("Grenade", transform.position);

            var hostile = Physics2D.OverlapCircleAll(transform.position, explosionRad, LayerMask.GetMask("Hostile"));
            if (hostile.Length > 0)
                foreach (var item in hostile)
                {
                    item.GetComponent<Entity>().OnHit(damage, transform);
                }

            Instantiate(explosion, transform.position, Quaternion.identity);
            isAttackAble = false;

            Destroy(gameObject);
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
