using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_Ayla : SingleFirePlayer
{
    protected override string fireSound => "Pistol_3";

    protected override void Skill()
    {
        ItemHealth(maxHP * 0.25f);
        isSkillActive = false;
    }
}
