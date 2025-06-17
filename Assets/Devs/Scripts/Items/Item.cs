using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffect
{
    extraSpaces,
    negativeSpaces,
}

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Sprite Icon;
    public ItemEffect Effect;
    public string Name;
    public string Description;
}
