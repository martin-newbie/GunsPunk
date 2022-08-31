using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player4_Jade : BurstFirePlayer
{
    protected override string fireSound => "Assault_4";

    protected override void Skill()
    {
        StartCoroutine(SkillCoroutine());
    }

    IEnumerator SkillCoroutine()
    {
        isActing = true;
        invincible = true;

        anim.SetTrigger("SkillTrigger");
        float timer = 2f;
        while (timer > 0f)
        {
            feverValue = (timer / 2f) * 100f;
            timer -= Time.deltaTime;
            yield return null;
        }

        isActing = false;
        invincible = false;
        isSkillActive = false;
    }
}
