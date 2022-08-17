using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeDrone : Drone
{

    [Header("Grenade Drone")]
    public float b_damage;
    public float b_speed;
    public Bullet grenade;
    public ParticleSystem shootEffect;

    protected override Bullet Attack()
    {
        Bullet temp = Instantiate(grenade, shootPos.position, Quaternion.identity);
        AudioManager.Instance.PlayEffectSound("Launcher");
        temp.Init(b_speed, b_damage, player);
        shootEffect.Play();
        return temp;

    }
}
