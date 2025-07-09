using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class LobbyPlayerInput : MonoBehaviourPun
{
    public PlayerClass PlayerData;
    
    private LobbyInputs inputs;

    private void Start()
    {
        if (photonView.IsMine)
        {
            PlayerData = new PlayerClass(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);
        }

        LobbyManager.Instance.AddPlayer(PlayerData);
    }

    private void OnEnable()
    {
        if(!photonView.IsMine){return;}
        
        inputs = new LobbyInputs();
        
        inputs.LobbyMap.Ready.performed += ToggleReady;
        inputs.Enable();
    }
    
    private void OnDisable()
    {
        if(!photonView.IsMine){return;}
        inputs.Disable();
        inputs.LobbyMap.Ready.performed -= ToggleReady;
    }

    public void ToggleReady(InputAction.CallbackContext ctx)
    {
        PlayerData.IsReady = !PlayerData.IsReady;
        LobbyManager.Instance.LocalPlayerHitReady(PlayerData);
    }

    
}