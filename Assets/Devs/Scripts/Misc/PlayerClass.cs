using System;
using UnityEngine;

[Serializable]
public class PlayerClass
{
    [SerializeField] private int id;                                                // Serialized for JSON compatibility
    [SerializeField] private string name;                                           // Serialized for JSON compatibility
    [SerializeField] private bool isReady = false;                                  // Default to not ready
    [SerializeField] private PlayermodelClass playermodel = new PlayermodelClass(); // Default initialization

    public PlayerClass(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public int Id
    {
        get => id;
        set => id = value; // Allow setting if needed
    }

    public string Name
    {
        get => name;
        set => name = value; // Allow setting if needed
    }

    public bool IsReady
    {
        get => isReady;
        set => isReady = value;
    }

    public PlayermodelClass Playermodel
    {
        get => playermodel;
        set => playermodel = value ?? new PlayermodelClass(); // Ensure non-null assignment
    }
}
