using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static LobbyManager Instance;
    
    private PlayerClass[] Players = new PlayerClass[4];
    public UIDocument UiDoc;
    public int ReadyCount = 0;

    [SerializeField]
    private int _amountOfReadyPlayersNeeded = 1; // Set the number of players needed to start the game
    
    public PlayermodelClass[] ModelOptions = new PlayermodelClass[4]
    {
        new PlayermodelClass(),
        new PlayermodelClass(),
        new PlayermodelClass(),
        new PlayermodelClass()
    };
    
    private int _nextModelIndex = 0; // Index to track the next available model
    
    // This array holds the visual elements for each player in the lobby.
    private VisualElement[] _playerObjects;
    private Label _countdownElement;
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
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(ReadyCount);
        }
        else
        {
            // Network player, receive data
            ReadyCount = (int)stream.ReceiveNext();
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
        
        _countdownElement = root.Q<Label>("Counter");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        AssigModelToPlayer(newPlayer);
    }

    private void AssigModelToPlayer(Player player)
    {
        if (_nextModelIndex >= ModelOptions.Length)
        {
            Debug.LogWarning("No more models available to assign.");
            return;
        }

        // Assign the next available model
        PlayermodelClass assignedModel = ModelOptions[_nextModelIndex];
        assignedModel.IsSelected = true;

        // Update the player's custom properties
        Hashtable properties = new Hashtable
        {
            {"SelectedModel", assignedModel.Model.name}
        };
        player.SetCustomProperties(properties);

        Debug.Log($"Assigned model {assignedModel.Model.name} to player {player.NickName}.");

        _nextModelIndex++; // Move to the next model
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        
        // Check if the player has a selected model
        if (changedProps.ContainsKey("SelectedModel"))
        {
            string modelName = (string)changedProps["SelectedModel"];
            Debug.Log($"Player {targetPlayer.NickName} selected model: {modelName}");
            
            // Find the model in ModelOptions
            foreach (PlayermodelClass model in ModelOptions)
            {
                if (model.Model.name == modelName)
                {
                    // Update the player's visual element with the selected model
                    VisualElement playerObject = _playerObjects[targetPlayer.ActorNumber - 1];
                    playerObject.Q<VisualElement>("Icon").style.backgroundImage = model.Icon;
                    
                    model.IsSelected = true;
                    return;
                }
            }
        }
    }
    
    [PunRPC]
    public void AddPlayerToArray(string serializedPlayer)
    {
        PlayerClass receivedPlayer = JsonUtility.FromJson<PlayerClass>(serializedPlayer);
        
        Players[receivedPlayer.Id] = receivedPlayer;
    }
    
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
    
    /// <summary>
    /// Only call this method at the start of the game to get a player model that is not yet selected.
    /// </summary>
    
    // public PlayermodelClass GetStartingPlayerModel(PlayerClass playerClass)
    // {
    //     foreach (PlayermodelClass model in ModelOptions)
    //     {
    //         if (model.IsSelected == false)
    //         {
    //             photonView.RPC("SetModelSelectedState", RpcTarget.AllBuffered, true, model.Model.name);
    //             return model;
    //         }
    //     }
    //     
    //     Debug.LogWarning("No available player models found.");
    //     return null; // Return null or handle the case where no models are available
    // }
    //
    [PunRPC]
    private void SetModelSelectedState(bool state, string modelName)
    {
        foreach (PlayermodelClass model in ModelOptions)
        {
            if (model.Model.name == modelName)
            {
                model.IsSelected = state;
                return;
            }
        }
        
        Debug.LogWarning($"Model {modelName} not found in ModelOptions.");
    }
    
    [PunRPC]
    public void CheckIfReady(string serializedPlayer)
    {
        // Deserialize the player data from the JSON string
        PlayerClass receivedPlayer = JsonUtility.FromJson<PlayerClass>(serializedPlayer);
        
        // Get the player's visual element from the _playerObjects array using their ID
        VisualElement playerIcon = _playerObjects[receivedPlayer.Id].Q<VisualElement>("Icon");
        
        // Check how many players are ready
        if (receivedPlayer.IsReady)
        {
            ReadyCount += 1;
            Debug.Log($"{receivedPlayer.Name} is ready! Total ready players: {ReadyCount}");
            
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
            ReadyCount -= 1;
            Debug.Log($"{receivedPlayer.Name} is not ready! Total ready players: {ReadyCount}");
            
            playerIcon.style.borderTopWidth = 0f;
            playerIcon.style.borderBottomWidth = 0f;
            playerIcon.style.borderLeftWidth = 0f;
            playerIcon.style.borderRightWidth = 0f;
        }

        if(ReadyCount >= _amountOfReadyPlayersNeeded)
        {
            Debug.Log("All players are ready!");
            // Load the game scene or perform any other action needed when all players are ready.
            StartCoroutine(PlayersAreReady());
        }
        else
        {
            Debug.Log("Not all players are ready yet.");
            
            _countdownElement.visible = false;
            StopAllCoroutines();
        } 
    }

    public IEnumerator PlayersAreReady()
    {
        int countdown = 3;
        while (countdown <= 3 && ReadyCount == _amountOfReadyPlayersNeeded)
        {
            
            _countdownElement.visible = true;
            _countdownElement.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            if (countdown <= 1 && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Loading Scene");
                PhotonNetwork.LoadLevel("Board");
            }
            countdown -= 1;
        }
    }
    
}
