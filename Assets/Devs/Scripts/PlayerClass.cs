using System;
using UnityEngine;

[Serializable]
public class PlayerClass
{   
    public PlayerClass(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
    
    public int Id { get; private set; }
    public string Name { get; private set; }
}
