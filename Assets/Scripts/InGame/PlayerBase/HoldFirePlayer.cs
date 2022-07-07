using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldFirePlayer : PlayerBase
{
    [Header("Hold Fire Player")]
    public GameObject gaugeObject;
    public float maxHold = 5f;
    public float curHold;
    public bool isHold;

    Image gaugeUI;
    GameObject gaugeObj;

    protected override void Start()
    {
        base.Start();
        gaugeObj = Instantiate(gaugeObject, InGameManager.Instance.canvas.transform);
        gaugeUI = gaugeObj.GetComponentsInChildren<Image>()[1];
    }

    protected override void Update()
    {
        base.Update();


        if (isHold)
        {
            if (curHold <= maxHold)
                curHold += Time.deltaTime;
            else
            {
                Bullet temp = FireBullet();
                temp.notDestroy = true;
                curHold = 0f;
            }
        }

        gaugeUI.fillAmount = curHold / maxHold;
        gaugeObj.transform.position = transform.position + new Vector3(0, 1.3f, 0);
    }

    public override void OnAttackEnd()
    {
        FireBullet();
        curHold = 0f;
        isHold = false;
    }

    public override void OnAttackStart()
    {
        isHold = true;
    }

    Bullet FireBullet()
    {
        Bullet _bullet = Instantiate(bullet, FirePos.position, Quaternion.identity);
        _bullet.Init(speed / 2f + speed * (curHold / maxHold), damage / 2 + damage * (curHold / maxHold));

        return _bullet;
    }
}
