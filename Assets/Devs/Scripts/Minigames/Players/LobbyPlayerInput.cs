using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class LobbyPlayerInput : MonoBehaviourPun
{
    public PlayerClass Player;
    
    private VisualElement _playerObject;
    
    private LobbyInputs _lobbyInputs;
    
    private PlayermodelClass previousModel;
    private void Start()
    {
        Player = LobbyManager.Instance.GetPlayerClass(photonView);
        
        _playerObject = LobbyManager.Instance.GetPlayerObject(Player);
        
        Player.Playermodel = LobbyManager.Instance.GetStartingPlayerModel(Player);
        previousModel = Player.Playermodel;
        
        _playerObject.Q<VisualElement>("Icon").style.backgroundImage = Player.Playermodel.Icon;
        
        DataManager.Instance.MyPlayerClass = Player;
    }

    private void OnEnable()
    {
        _lobbyInputs = new LobbyInputs();
        
        _lobbyInputs.LobbyMap.Next.performed += OnNext;
        _lobbyInputs.LobbyMap.Previous.performed += OnPrevious;
        _lobbyInputs.LobbyMap.Ready.performed += OnReady;
        _lobbyInputs.Enable();
    }
    private void OnDisable()
    {
        _lobbyInputs.Disable();
    }
    
    public void OnNext(InputAction.CallbackContext ctx)
    {
        
    }
    
    public void OnPrevious(InputAction.CallbackContext ctx)
    {

    }

    public void OnReady(InputAction.CallbackContext ctx)
    {
        photonView.RPC("DoReadyLogic", RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    private void DoReadyLogic()
    {
        // Debug.Log("Ready Button Pressed");
        // Toggle the player's ready state
        Player.IsReady = !Player.IsReady;
        
        // Parse player to Json
        string serializedPlayer = JsonUtility.ToJson(Player);
        
        // Get LobbyUI's photonView and call the RPC to update the ready state for all players
        PhotonView lobbyUIPhotonView = LobbyManager.Instance.photonView;
        lobbyUIPhotonView.RPC("CheckIfReady", RpcTarget.AllBuffered, serializedPlayer);
    }
}
