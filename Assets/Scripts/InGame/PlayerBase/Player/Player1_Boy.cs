using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_Boy : SingleFirePlayer
{
    [Header("Boy")]
    public Granade granade;
    public Vector2 g_force;
    public float g_torque;
    public float g_damage;

    protected override void Skill()
    {
        Granade temp = Instantiate(granade, transform.position, Quaternion.identity);
        temp.Init(g_force, g_torque, g_damage);
        isSkillActive = false;
    }
}
