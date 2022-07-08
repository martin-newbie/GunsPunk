using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{

    public Canvas canvas;
    public PlayerBase CurPlayer;

    public GaugeContainer GaugePrefab;

    public GaugeContainer SpawnGaugeBar()
    {
        GaugeContainer temp = Instantiate(GaugePrefab, canvas.transform);
        return temp;
    }

}
