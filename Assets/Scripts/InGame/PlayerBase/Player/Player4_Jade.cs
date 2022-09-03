using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player4_Jade : BurstFirePlayer
{
    protected override string fireSound => "Assault_4";

    [Header("Jade")]
    public SpriteRenderer chain;
    public Transform anchor;
    public Transform endPos;
    Vector3 startPos;

    protected override void Start()
    {
        base.Start();
        startPos = anchor.localPosition;
    }

    protected override void Update()
    {
        base.Update();
        chain.size = new Vector2(anchor.localPosition.x - startPos.x, 0.14f);
    }

    protected override void Skill()
    {
        StartCoroutine(SkillCoroutine());
    }

    IEnumerator SkillCoroutine()
    {
        moveAble = false;
        invincible = true;
        anim.SetBool("AttackAble", false);
        anim.SetTrigger("SkillTrigger");

        yield return new WaitForSeconds(0.45f);
        anchor.gameObject.SetActive(true);

        float timer = 0f;
        Vector3 originPos = anchor.localPosition;

        while (timer < 1f)
        {
            anchor.localPosition = Vector3.Lerp(originPos, endPos.localPosition, 1- Mathf.Pow(1 - (timer / 1f), 3));
            timer += Time.deltaTime;
            yield return null;
        }

        anim.SetTrigger("SkillPull");

        timer = 1.3f;
        originPos = anchor.localPosition;
        while (timer > 0f)
        {
            anchor.localPosition = Vector3.Lerp(startPos, originPos, 1 - Mathf.Pow(1 - (timer / 1.3f), 3));
            timer -= Time.deltaTime;
            yield return null;
        }

        anim.SetTrigger("SkillEnd");

        yield return null;
        anchor.gameObject.SetActive(false);
        anim.SetBool("AttackAble", true);

        moveAble = true;
        invincible = false;
        isSkillActive = false;
    }
}
