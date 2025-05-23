using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    
    public UIDocument _uiDocument;
    
    private VisualElement[] _playerObjects;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        // Get the root visual element
        VisualElement root = _uiDocument.rootVisualElement;
        // Find the player objects in the hierarchy
        _playerObjects = new VisualElement[4];
        
        for (int i = 0; i < _playerObjects.Length; i++)
        {
            _playerObjects[i] = root.Q<VisualElement>("Player" + (i + 1));
        }
    }

    public VisualElement GetPlayerObject(PlayerClass playerClass)
    {
        _playerObjects[playerClass.id].Q<Label>("PlayerName").text = playerClass.name;
        return _playerObjects[playerClass.id];
    }
}
