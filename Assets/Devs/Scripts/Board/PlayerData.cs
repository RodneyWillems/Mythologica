using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public string PlayerName = null;
    public GameObject PlayerObject = null;
    public int Obols = 0;
    public int Ambrosias = 0;
    public List<Item> Items = new();
}
