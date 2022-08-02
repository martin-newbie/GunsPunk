using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_Girl : SingleFirePlayer
{
    protected override void Skill()
    {
        ItemHealth(maxHP * 0.25f);
        isSkillActive = false;
    }
}
