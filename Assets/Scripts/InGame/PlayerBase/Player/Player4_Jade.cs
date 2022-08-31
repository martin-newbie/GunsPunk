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
        yield return new WaitForSeconds(2f);

        isActing = false;
        invincible = false;
    }
}
