using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour, IRefresh
{
    public Text CountTxt;
    public Text CostTxt;
    public Image[] ItemImg;

    ItemInfo info;

    public void Init(ItemInfo _info)
    {
        ShopUIManager.Instance.AddRefreshAble(this);
        info = _info;
        Refresh();

        for (int i = 0; i < ItemImg.Length; i++)
        {
            if (i == info.idx) ItemImg[i].gameObject.SetActive(true);
            else ItemImg[i].gameObject.SetActive(false);
        }
    }

    public void Refresh()
    {
        CostTxt.text = string.Format("{0:0,#}", info.cost);
        CostTxt.color = GameManager.Instance.coin >= info.cost ? Color.white : Color.red;

        CountTxt.text = info.count.ToString();
    }

    public void BuyButton()
    {
        if(info.cost <= GameManager.Instance.coin)
        {
            AudioManager.Instance.PlayUISound("Cash");
            info.count++;

            GameManager.Instance.coin -= info.cost;

            ShopUIManager.Instance.Refresh();
        }
    }
}
