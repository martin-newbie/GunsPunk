using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleDrone : Drone
{

    [Header("Rifle Drone")]
    public float b_damage;
    public float b_speed;
    public Bullet bullet;
    public ParticleSystem shootEffect;

    protected override Bullet Attack()
    {
        Bullet temp = Instantiate(bullet, shootPos.position, Quaternion.identity);
        shootEffect.Play();

        AudioManager.Instance.PlayEffectSound("Assault_2");
        temp.Init(b_speed, b_damage, player);
        return temp;
    }
}
