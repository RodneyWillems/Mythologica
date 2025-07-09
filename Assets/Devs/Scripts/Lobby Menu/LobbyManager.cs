using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using UnityEngine.UIElements;

[Serializable]
public class PlayerListWrapper
{
    [SerializeField] public List<PlayerClass> Players = new List<PlayerClass>();
}

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    
    [SerializeField] int _readyPlayersNeeded = 4;
    
    public PlayermodelClass[] Playermodels = new PlayermodelClass[4];
    
    private int _playerModelIndex = 0;

    private List<PlayerClass> _players = new List<PlayerClass>();
    
    // UI element
    private UIDocument _uiDocument;
    
    // Visual elements for player objects
    private VisualElement[] _playerObjects;
    
    // Countdown element
    private Label _countdownElement;
    
    // Custom properties text for debugging
    [SerializeField] 
    private TextMeshProUGUI customPropertiesText;

    private Dictionary<int, PlayerClass> _playersData = new Dictionary<int, PlayerClass>();
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.Instantiate("LobbyPlayer", Vector3.zero, Quaternion.identity, 0);
        
        _playersData[PhotonNetwork.LocalPlayer.ActorNumber] = new PlayerClass(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);
        
        AssignModelToLocalPlayer();
        UpdateLobbyUI();
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
        
        _countdownElement = root.Q<Label>("Counter");
    }

    private void AssignModelToLocalPlayer()
    {
        // Find first unselected model
        PlayermodelClass availableModel = Playermodels.FirstOrDefault(model => !model.IsSelected);

        if (availableModel != null)
        {
            // Mark the model as selected
            availableModel.IsSelected = true;
            
            // Assign the model to the local player
            PlayerClass playerData = _playersData[PhotonNetwork.LocalPlayer.ActorNumber];
            playerData.PlayerModel = availableModel;
            
            // Store the model in Photon's custom properties
            Hashtable customProperties = new Hashtable
            {
                { "PlayerModel", availableModel.Model.name },
                {"Ready", false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
        else
        {
            Debug.LogError("No available models to assign to the local player.");
        }
    }
    
    public void LocalPlayerHitReady(PlayerClass player)
    {
        player.IsReady = !player.IsReady;
        
        Hashtable customProperties = new Hashtable
        {
            { "Ready", player.IsReady }
        };
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPLayer, Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("Ready"))
        {
            CheckAllReady();
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Add the new player to _playersData with a default PlayerClass
        _playersData[newPlayer.ActorNumber] = new PlayerClass(newPlayer.ActorNumber, newPlayer.NickName);
        
        // Re check ready state for all players
        CheckAllReady();
    }

    private void CheckAllReady()
    {
        UpdateLobbyUI();
        if (PhotonNetwork.CurrentRoom.PlayerCount < _readyPlayersNeeded)
            return;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!p.CustomProperties.TryGetValue("Ready", out object ready) || !(bool)ready)
                return;
        }

        // All players are ready
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Board");
        }
    }
    
    public void AddPlayer(PlayerClass player)
    {
        _players.Add(player);
        UpdateLobbyUI();  
        
        if(PhotonNetwork.CurrentRoom == null){Debug.Log("Called before room"); return;}
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
        {
            { "PlayerList", JsonUtility.ToJson(new PlayerListWrapper { Players = _players }) }
        });
    }
    
    public void RemovePlayer(PlayerClass player)
    {
        _players.Remove(player);
        UpdateLobbyUI();
        
        if(PhotonNetwork.CurrentRoom == null){return;}
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
        {
            { "PlayerList", JsonUtility.ToJson(new PlayerListWrapper { Players = _players }) }
        });
    }
    
    private void UpdateLobbyUI()
    {
        // Update the UI to reflect the current player list and ready states
        for (int i = 0; i < _playerObjects.Length; i++)
        {
            if (i < _players.Count && _players[i] != null)
            {
                _playerObjects[i].style.display = DisplayStyle.Flex;
                _playerObjects[i].Q<Label>("PlayerName").text = _players[i].Name;
    
                VisualElement playerIcon = _playerObjects[i].Q<VisualElement>("Icon");
    
                if (_players[i].IsReady)
                {
                    // Change icons border to green if player is ready
                    playerIcon.style.borderTopColor = Color.green;
                    playerIcon.style.borderBottomColor = Color.green;
                    playerIcon.style.borderLeftColor = Color.green;
                    playerIcon.style.borderRightColor = Color.green;
    
                    playerIcon.style.borderTopWidth = 2f;
                    playerIcon.style.borderBottomWidth = 2f;
                    playerIcon.style.borderLeftWidth = 2f;
                    playerIcon.style.borderRightWidth = 2f;
                }
                else
                {
                    playerIcon.style.borderTopWidth = 0f;
                    playerIcon.style.borderBottomWidth = 0f;
                    playerIcon.style.borderLeftWidth = 0f;
                    playerIcon.style.borderRightWidth = 0f;
                }
            }
            else
            {
                // _playerObjects[i].style.display = DisplayStyle.None;
            }
        }
    }
    // public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    // {
    //     Debug.Log("OnRoomPropertiesUpdate");
    //     base.OnRoomPropertiesUpdate(propertiesThatChanged);
    //     if (propertiesThatChanged.TryGetValue("PlayerList", out var serializedPlayers))
    //     {
    //         PlayerListWrapper wrapper = JsonUtility.FromJson<PlayerListWrapper>(serializedPlayers.ToString());
    //         _players = wrapper.Players;
    //         UpdateLobbyUI();
    //     }
    //     UpdateCustomPropertiesUI(); // Update UI for debugging purposes
    // }

    
    //
    // [PunRPC]
    // private void UpdateReadyState(int playerId, bool isReady)
    // {
    //     foreach (PlayerClass player in _players)
    //     {
    //         if (player.Id == playerId)
    //         {
    //             player.IsReady = isReady;
    //             
    //         }
    //         
    //         if(PhotonNetwork.CurrentRoom == null){continue;}
    //         PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
    //         {
    //             { "PlayerList", JsonUtility.ToJson(new PlayerListWrapper { Players = _players }) }
    //         });
    //     }
    //     
    //     UpdateCustomPropertiesUI(); // Update UI for debugging purposes
    // }
    //
    // public bool AreAllPlayersReady()
    // {
    //     foreach (var player in _players)
    //     {
    //         if (!player.IsReady) return false;
    //     }
    //     return true;
    // }
    //
    // public void StartGame()
    // {
    //     if (AreAllPlayersReady() && PhotonNetwork.IsMasterClient)
    //     {
    //         // Load the game scene
    //         PhotonNetwork.CurrentRoom.IsOpen = false; // Prevent new players from joining
    //         PhotonNetwork.CurrentRoom.IsVisible = false; // Hide the room from the lobby
    //         PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
    //         {
    //             { "GameStarted", true }
    //         });
    //         
    //         // Load the game scene
    //         PhotonNetwork.LoadLevel("Board");
    //     }
    //     else if (!PhotonNetwork.IsMasterClient)
    //     {
    //         Debug.LogWarning("Only the master client can start the game.");
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Not all players are ready.");
    //     }
    //     {
    //         PhotonNetwork.LoadLevel("Board");
    //     }
    // }
    //
    
    //
    // public override void OnPlayerLeftRoom(Player otherPlayer)
    // {
    //     _players.RemoveAt(otherPlayer.ActorNumber);
    //     UpdateLobbyUI();
    //     
    //     UpdateCustomPropertiesUI(); // Update UI for debugging purposes
    //     
    //     if(PhotonNetwork.CurrentRoom == null){return;}
    //     PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
    //     {
    //         { "PlayerList", JsonUtility.ToJson(new PlayerListWrapper { Players = _players }) }
    //     });
    // }
    //
    // // DEBUGGING FUNCTIONS
    // void UpdateCustomPropertiesUI()
    // {
    //     string propertiesDisplay = "Custom Properties:\n";
    //     foreach (var property in PhotonNetwork.CurrentRoom.CustomProperties)
    //     {
    //         propertiesDisplay += $"{property.Key}: {property.Value}\n";
    //     }
    //     customPropertiesText.text = propertiesDisplay;
    // }
}