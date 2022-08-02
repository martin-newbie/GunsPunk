using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpperBar : Singleton<UpperBar>
{

    public Text CoinTxt;
    public Text EnergyTxt;

    void Update()
    {
        CoinTxt.text = string.Format("{0:0,#}", GameManager.Instance.curCoin);
        EnergyTxt.text = string.Format("{0:0,#}", GameManager.Instance.energy);
    }

    public void OpenSetting()
    {

    }
}
