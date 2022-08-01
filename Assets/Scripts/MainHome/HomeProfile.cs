using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeProfile : HomeUI
{
    public Image ExpBar;
    public Image Icon;
    public Text Name;
    public Text Level;

    public void SetUI(string _name, int _level, Sprite icon)
    {
        Name.text = _name;
        Level.text = "Lv. " + _level.ToString();

        Icon.sprite = icon;
        SetPlayerExpBar();
    }

    public void SetPlayerExpBar()
    {
        ExpBar.fillAmount = GameManager.Instance.userExp / GameManager.Instance.userMaxExp;
    }
}
