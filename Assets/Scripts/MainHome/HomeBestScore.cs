using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeBestScore : HomeUI
{
    public Text bestScore;

    public void SetUI(string score)
    {
        bestScore.text = score + "M";
    }
}
