using System;
using Photon.Realtime;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    
    public PlayerClass MyPlayerClass;
    private void Awake()
    {
        // Ensure that there is only one instance of DataManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
