using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="Item Information", order =int.MinValue), System.Serializable]
public class ItemInfo : ScriptableObject
{
    public int idx;
    public int count;
    public int cost;
}
