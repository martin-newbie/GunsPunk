using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_Boy : SingleFirePlayer
{
    [Header("Boy")]
    public Grenade grenade;
    public Vector2 g_force;
    public float g_torque;
    public float g_damage;

    protected override void Skill()
    {
        Grenade temp = Instantiate(grenade, FirePos.transform.position, Quaternion.identity);
        temp.Init(g_force, g_torque, g_damage);
        isSkillActive = false;
    }
}
