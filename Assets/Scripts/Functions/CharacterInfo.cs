using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterInfo", menuName ="Character Menu", order =int.MinValue)]
public class CharacterInfo : ScriptableObject
{
    public int idx;
    public int level;
    public readonly int maxLevel = 5;

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

}
