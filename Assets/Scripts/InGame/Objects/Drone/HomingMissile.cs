using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : Bullet
{
    Transform target;
    Transform start;

    [Header("Homing Missile")]
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] Transform bulletObj;

    public float radius;
    public GameObject explosion;
    [SerializeField] bool active;

    Vector2[] point = new Vector2[4];

    [SerializeField, Range(0, 1)] float t = 0;
    [SerializeField] float posA = 0.55f;
    [SerializeField] float posB = 0.45f;


    public void Init(Transform _start, Transform _target, float _speed)
    {
        bulletObj.gameObject.SetActive(true);
        target = _target;
        speed = _speed;
        active = true;
        t = 0f;

        point[0] = _start.position;
        point[1] = PointSetting(_start.position);
        point[2] = PointSetting(_target.position);
        point[3] = _target.position;

        transform.position = point[0];
        smokeEffect.Play();
    }

    Vector2 PointSetting(Vector2 origin)
    {
        float x, y;
        x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
        y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;
        return new Vector2(x, y);
    }

    void DrawTrajectory()
    {
        transform.position = new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
            );
    }

    float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }

    protected override void OnBecameInvisible()
    {
    }

    protected override void Update()
    {
        FollowTarget();
    }

    protected override void AttackAction(Collider2D collision)
    {
        if (collision.transform == target)
        {
            active = false;
            AudioManager.Instance.PlayEffectSound("Grenade");
            Instantiate(explosion, transform.position, Quaternion.identity);

            Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Hostile"));
            foreach (var item in objs)
            {
                Entity temp;
                if (item.TryGetComponent(out temp))
                {
                    temp.OnHit(damage, transform);
                }
            }
        }
    }



    private void FixedUpdate()
    {
        if (!active) return;

        if (t > 1)
        {
            bulletObj.gameObject.SetActive(false);
            smokeEffect.Stop();
            return;
        }
        t += Time.deltaTime * speed;
        DrawTrajectory();
    }

    void FollowTarget()
    {
        var dir = bulletObj.position - target.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bulletObj.rotation = Quaternion.Euler(0, 0, angle + 180);
    }
}
