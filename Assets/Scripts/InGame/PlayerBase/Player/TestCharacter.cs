using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : AutoFirePlayer
{
    protected override void Skill()
    {
        isSkillActive = false;
    }
}
