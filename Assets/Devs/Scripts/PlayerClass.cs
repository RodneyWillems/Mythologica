using UnityEngine;

public class PlayerClass
{   
    public PlayerClass(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
    
    public int id { get; private set; }
    public string name { get; private set; }
}
