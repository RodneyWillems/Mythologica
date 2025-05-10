using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[Serializable]
public class IconPreset
{
    public Dictionary<string, Texture> iconDict = new Dictionary<string, Texture>();
    
    public string name;
    public string[] keys;
    public Texture[] icons;
    
    public void Init()
    {
        Debug.Log(keys.Length);
        Debug.Log(icons.Length);
        for (int i = 0; i < keys.Length; i++)
        {
            iconDict.Add(keys[i], icons[i]);
        }
    }
}
public class ButtonIconManager : MonoBehaviour
{
    public static ButtonIconManager Instance { get; private set; }
    
    public IconPreset playstationPreset;
    public IconPreset xboxPreset;
    public IconPreset switchPreset;
    public IconPreset pcPreset;

    private void Awake()
    {
        // Initialize the icon presets
        //playstationPreset.Init();
        xboxPreset.Init();
        //switchPreset.Init();
        //pcPreset.Init();
        
        // Singleton pattern to ensure only one instance of ButtonIconManager exists

        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get the icon preset based on the current control scheme
    public IconPreset GetPreset(PlayerInput user)
    {
        switch (user.currentControlScheme)
        {
            case "Xbox":
                return xboxPreset;
            case "Switch":
                return switchPreset;
            case "PlayStation":
                return playstationPreset;
            case "Keyboard":
                return pcPreset;
        }
        return pcPreset;
    }
}
