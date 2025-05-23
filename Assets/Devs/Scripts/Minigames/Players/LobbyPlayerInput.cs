using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class LobbyPlayerInput : MonoBehaviourPun, Lobby.ILobbyMapActions
{
    public PlayerClass playerClass;
    
    private VisualElement _playerObject;
    private void Start()
    {
        playerClass = new PlayerClass(PhotonNetwork.CurrentRoom.PlayerCount - 1, photonView.Owner.NickName);
        
        _playerObject = LobbyManager.Instance.GetPlayerObject(playerClass);
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
