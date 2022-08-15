using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="Item Information", order =int.MinValue)]
public class ItemInfo : ScriptableObject
{
    public int idx;
    public int count;
    public int cost;
}
