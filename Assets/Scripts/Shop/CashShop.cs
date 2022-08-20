using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType
{
    Kor,
    Eng
}

public class CashShop : MonoBehaviour
{
    public LanguageType language;
    public string path => "Sprite/Shop/coin shop ui/";
    public Sprite[] buttonSprites;
    public Sprite[] backgroundSprites;
    public Transform content;

    public CashContainer cashPrefab;

    [Header("Value")]
    public int[] coinValue = new int[3];
    public int[] energyValue = new int[3];

    void Start()
    {
        buttonSprites = Resources.LoadAll<Sprite>(path + "Buttons");
        backgroundSprites = Resources.LoadAll<Sprite>(path + "Backgrounds");

        int bgIdx = 0;
        int start = language == LanguageType.Eng ? 0 : 6;
        for (int i = start; i < 6 + start; i += 2)
        {
            CashContainer temp = Instantiate(cashPrefab, content);
            int idx = bgIdx;
            temp.Init(buttonSprites[i], buttonSprites[i + 1], backgroundSprites[bgIdx], ()=> { BuyItem(idx); });

            bgIdx++;
        }
    }

    public void BuyItem(int idx)
    {
        GameManager.Instance.coin += coinValue[idx];
        GameManager.Instance.energy += energyValue[idx];

        ShopUIManager.Instance.Refresh();
    }

}
