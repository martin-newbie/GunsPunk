using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{

    public Canvas canvas;
    public PlayerBase CurPlayer;

    public GaugeContainer HpBar;

    public GaugeContainer SpawnHpBar()
    {
        return null;
    }

}
