using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDrone : Drone
{
    [Header("Homing Drone")]
    public HomingMissile missile;
    public bool shootAble;

    protected override void Update()
    {
        base.Update();

        if (shootAble)
        {
            var monsters = InGameManager.Instance.CurMonsters;
            if(monsters.Count > 0)
            {
                var target = monsters[Random.Range(0, monsters.Count)];
                HomingMissile temp = Instantiate(missile, shootPos.position, Quaternion.identity);
                temp.Init(shootPos, target.GetTransform(), speed);

                shootAble = false;
            }

        }
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
