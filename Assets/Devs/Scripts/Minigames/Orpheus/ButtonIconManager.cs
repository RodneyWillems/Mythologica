using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[Serializable]
public class IconPreset
{
    public Sprite[] icons;
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
        Instance = this;

        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
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
                break;
            case "Switch":
                return switchPreset;
                break;
            case "PlayStation":
                return playstationPreset;
                break;
            case "Keyboard":
                return pcPreset;
                break;
        }
        return pcPreset;
    }
}
