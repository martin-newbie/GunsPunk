using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : Entity
{
    public string hitSound = "MetalHit_";
    public string crashSound = "MetalCrash";
    public HostileAttack atkCol;
    public SpriteRenderer hurdleObj;
    public ParticleSystem HitParticle;
    public float healthPackChance;
    public float ammunitionChance;
    public float explosionChance;

    [Header("Value")]
    public float duration;
    public float amount;
    public Vector3 localPos;

    [Header("Debris")]
    public Rigidbody2D[] Debris;

    protected override void Awake()
    {
        base.Awake();
        OnHitAction = OnHit;
        OnDestroyAction = OnDie;
        atkCol.Init(damage, OnDie);

        localPos = hurdleObj.transform.localPosition;
    }

    private void Update()
    {
        if (InGameManager.Instance.isGameActive)
        {
            moveSpeed = InGameManager.Instance.objectSpeed;
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
    Coroutine shakeCoroutine;
    public override void OnHit(float damage, Transform hit)
    {
        base.OnHit(damage, hit);

        AudioManager.Instance.PlayEffectSound(hitSound + Random.Range(1, 4).ToString(), transform.position);
        HitParticle.Play();

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(HitCoroutine(duration, amount));
    }

    IEnumerator HitCoroutine(float duration, float amount)
    {
        float timer = duration;

        while (timer > 0f)
        {
            float amt = amount * (timer / duration);
            Vector3 randPos = localPos + (Vector3)(Random.insideUnitCircle * amt);

            hurdleObj.transform.localPosition = randPos;

            timer -= Time.deltaTime;
            yield return null;
        }

        hurdleObj.transform.localPosition = localPos;

        yield break;
    }

    void OnDie()
    {
        AudioManager.Instance.PlayEffectSound(crashSound, transform.position);

        GameManager.Instance.destroyedObjectCnt++;

        if (Random.Range(0, 100) < healthPackChance)
            InGameManager.Instance.SpawnRandomItem(transform.position, 1);
        else if (Random.Range(0, 100) < ammunitionChance)
            InGameManager.Instance.SpawnRandomItem(transform.position, 0);
        else if (Random.Range(0f, 100f) < explosionChance)
        {
            Explosion();
        }

        foreach (var item in Debris)
        {
            item.gameObject.SetActive(true);
            item.AddForce(new Vector2(Random.Range(-6, 6), Random.Range(4, 10)), ForceMode2D.Impulse);
            item.AddTorque(Random.Range(-5f, 5f));
            Destroy(item.gameObject, Random.Range(5, 15));
        }

        GetComponent<EnemyHp>().DestroyGauge();
        Destroy(GetComponent<Collider2D>());
        hurdleObj.gameObject.SetActive(false);
        Destroy(atkCol);
    }

    void Explosion()
    {
        Explosion temp = InGameManager.Instance.SpawnExplosion(transform.position);
    }
}