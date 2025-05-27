using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class LobbyPlayerInput : MonoBehaviourPun, Lobby.ILobbyMapActions
{
    public PlayerClass Player;
    
    private VisualElement _playerObject;
    private void Start()
    {
        Player = LobbyManager.Instance.GetPlayerClass(photonView);
        
        _playerObject = LobbyManager.Instance.GetPlayerObject(Player);
    }
    public void OnNext(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
        }
    }
    
    public void OnPrevious(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
        }
    }
}
