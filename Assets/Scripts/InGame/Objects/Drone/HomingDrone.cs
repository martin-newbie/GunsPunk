using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDrone : Drone
{
    [Header("Homing Drone")]
    public float b_speed;
    public float b_damage;
    public HomingMissile missile;
    public bool shootAble;

    protected override void Update()
    {
        base.Update();

        if (shootAble)
        {
            var monsters = InGameManager.Instance.CurMonsters;
            if (monsters.Count > 0)
            {
                var target = monsters[Random.Range(0, monsters.Count)];
                HomingMissile temp = Instantiate(missile, shootPos.position, Quaternion.identity);
                temp.Init(shootPos, target.GetTransform(), b_speed, b_damage);
                anim.SetTrigger("AttackTrigger");
                shootAble = false;
            }

        }
    }

    protected override void DelayLogic()
    {
        if (!shootAble)
            base.DelayLogic();
    }

    protected override void Shoot()
    {
        shootAble = true;
    }

    protected override Bullet Attack()
    {
        return null;
    }
}
