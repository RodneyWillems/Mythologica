using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public static MultiplayerManager instance;

    [SerializeField] private byte _maxPlayersPerRoom = 4;

    private string _gameVersion = "2.0";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        // Singleton pattern to ensure only one instance of MultiplayerManager exists
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

        PhotonNetwork.JoinRandomOrCreateRoom(null, _maxPlayersPerRoom);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {

        PhotonNetwork.LoadLevel("Lobby");
    }

public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }
    
    public void ConnectToServer()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            // PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }

    public void CreateRoom(string roomName)
    {   
        // #Critical: We check if the room name is null or empty
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Room name is null or empty");
            return;
        }
        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayersPerRoom;
        roomOptions.PlayerTtl = 30 * 60 * 1000;  // 30 minutes in millisecond 
        
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    
    public void JoinRoom(string roomName)
    {
        // #Critical: We check if the room name is null or empty
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Room name is null or empty");
            return;
        }
        
        PhotonNetwork.JoinRoom(roomName);
    }

    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(10, 10, 300, 30), PhotonNetwork.CurrentRoom.PlayerCount.ToString());
    // }
}
