using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Character Menu", order = int.MinValue), System.Serializable]
public class CharacterInfo : ScriptableObject
{
    [Header("Status")]
    public int idx;
    public int level;
    public int trainingLevel;
    public float exp;
    public float maxExp => GetMaxExp();
    public readonly int maxTrainingLevel = 5;


    [Header("Dynamic Value")]
    public float HP;
    public float HPIncrease;
    public int Ammo;
    public int AmmoIncrease;
    public float Damage;
    public float DamageIncrease;
    public float Fever;
    public float FeverIncrease;
    public float bulletSpeed;

    [Header("Static Value")]
    public float rpm;
    public float SpreadPos;
    public float SpreadRot;
    public int AmmoItemValue;
    public float HPItemValue;

    [Header("About Buy")]
    public int cost;
    public bool isUnlocked;
    public ValueType valueType;
    public bool isSelected;
    public string description => GetString();

    public void GetExp(float _exp)
    {
        exp += _exp;
        while (true)
        {
            if (exp >= maxExp)
            {
                exp -= maxExp;
                level++;
            }
            else
            {
                break;
            }
        }
    }

    public bool TrainigAble()
    {
        bool levelAble = (level / 10) > trainingLevel;
        bool maxAble = trainingLevel < maxTrainingLevel;

        return levelAble && maxAble;
    }

    public float GetMaxExp()
    {
        float max = level * 60 + 100;
        return max;
    }

    public string GetString()
    {
        return "";
    }
}
