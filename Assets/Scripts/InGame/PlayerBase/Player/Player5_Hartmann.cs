using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player5_Hartmann : AutoFirePlayer
{
    protected override string fireSound => "Mp40_1";

    [Header("Hartmann")]
    public Bullet skillBullet;
    public float skillDelay = 12f;
    public float skillFireRate = 1800f;
    protected override float fireDelay => isSkillActive ? 1f / (skillFireRate / 60f) : base.fireDelay;

    float timer = 0f;

    protected override void Skill()
    {
        StartCoroutine(SkillCoroutine(skillDelay));
    }

    IEnumerator SkillCoroutine(float duration)
    {
        timer = duration;
        anim.SetInteger("AttackState", 1);

        while (timer > 0f)
        {
            feverValue = (timer / duration) * 100f;
            timer -= Time.deltaTime;
            yield return null;
        }

        feverValue = 0f;
        anim.SetInteger("AttackState", 0);
        isSkillActive = false;
    }

    protected override Bullet FireBullet()
    {
        if (!isSkillActive)
            return base.FireBullet();
        else
        {
            return FireSkillBullet();
        }
    }

    Bullet FireSkillBullet()
    {
        Bullet _bullet;
        timer -= 0.05f;
        anim.SetTrigger("AttackTrigger");

        _bullet = Instantiate(skillBullet, FirePos.position + new Vector3(0, Random.Range(-spread_pos, spread_pos)), Quaternion.Euler(0, 0, Random.Range(-spread_rot * 2, spread_rot * 2)));
        _bullet.Init(speed * 1.2f, damage, this);
        
        BulletShellEffect.Play();
        GunFireEffect?.Play();


        AudioManager.Instance.PlayEffectSound("MachineGun_1");
        InGameManager.Instance.GetRoundCoin(1);

        return _bullet;
    }
}
