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
    public string characterName;


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

    public void SetExp(float _exp)
    {
        exp += _exp;
        while (exp >= maxExp)
        {
            exp -= maxExp;
            level++;
        }
    }

    public bool TrainingAble()
    {
        bool levelAble = (level / 10) > trainingLevel;
        bool maxAble = trainingLevel < maxTrainingLevel;

        return levelAble && maxAble;
    }

    public float GetMaxExp(int? _level = null)
    {
        float max;

        if (_level != null)
            max = (int)_level * 60 + 100;
        else
            max = level * 60 + 100;

        return max;
    }

    public string GetString()
    {
        return "";
    }
}
