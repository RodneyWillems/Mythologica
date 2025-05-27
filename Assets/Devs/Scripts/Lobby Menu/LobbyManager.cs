using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    
    public PlayerClass[] Players = new PlayerClass[4];
    public UIDocument UiDoc;
    
    private VisualElement[] _playerObjects;
    private void Awake()
    {
        Instance = this;
        
        PhotonNetwork.IsMessageQueueRunning = false;
    }

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.Instantiate("LobbyPlayer", Vector3.zero, Quaternion.identity, 0);   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PhotonNetwork.Instantiate("LobbyPlayer", Vector3.zero, Quaternion.identity, 0);
        }
    }

    private void OnEnable()
    {
        UiDoc = GetComponent<UIDocument>();
        
        // Get the root visual element
        VisualElement root = UiDoc.rootVisualElement;
        // Find the player objects in the hierarchy
        _playerObjects = new VisualElement[4];
        
        for (int i = 0; i < _playerObjects.Length; i++)
        {
            _playerObjects[i] = root.Q<VisualElement>("Player" + (i + 1));
        }
    }

    // public override void OnJoinedRoom()
    // {
    //     PhotonNetwork.Instantiate("LobbyPlayer", Vector3.zero, Quaternion.identity, 0);    
    //     
    // }

    public PlayerClass GetPlayerClass(PhotonView photonView)
    {
        PlayerClass playerClass = new PlayerClass(photonView.CreatorActorNr - 1, photonView.Owner.NickName);
        return playerClass;
    }
    public VisualElement GetPlayerObject(PlayerClass playerClass)
    {
        _playerObjects[playerClass.Id].Q<Label>("PlayerName").text = playerClass.Name;
        return _playerObjects[playerClass.Id];
    }
}
