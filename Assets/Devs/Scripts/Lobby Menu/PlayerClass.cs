using UnityEngine;

[System.Serializable]
public class PlayerClass
{
    [SerializeField] public int Id;
    [SerializeField] public string Name;
    [SerializeField] public bool IsReady;
    [SerializeField] public PlayermodelClass PlayerModel;
    
    public PlayerClass(int id, string name)
    {
        Id = id;
        Name = name;
        IsReady = false;
        PlayerModel = null;
    }
}