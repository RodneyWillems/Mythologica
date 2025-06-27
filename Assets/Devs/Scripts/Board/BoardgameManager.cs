using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public enum TurnOrder
{
    Player1,
    Player2, 
    Player3, 
    Player4,
}

public class BoardgameManager : MonoBehaviourPunCallbacks
{
    public static BoardgameManager Instance { get; private set; }

    [Header("Players")]
    [SerializeField] private PlayerData[] _playerData;
    [SerializeField] private GameObject[] _playerObjects;

    private List<BoardPlayers> _boardPlayers = new();

    // Turns
    private TurnOrder _turnOrder;
    private BoardPlayers _lastPlayer;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        PhotonNetwork.IsMessageQueueRunning = false;
    }

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;

        string playerModelName = DataManager.Instance.MyPlayerClass.Playermodel.Model.name + "Player";
        foreach (GameObject playerObject in _playerObjects)
        {
            if (playerObject.name == playerModelName)
            {
                playerObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        _turnOrder = TurnOrder.Player1;
        _playerData = new PlayerData[4];
        for (int i = 0; i < 4; i++)
        {
            _playerData[i] = new()
            {
                Obols = 20
            };
        }

    }

    public void AddPlayer(BoardPlayers player)
    {
        _boardPlayers.Add(player);
        _playerData[_boardPlayers.Count - 1].PlayerName = player.name;
        _playerData[_boardPlayers.Count - 1].PlayerObject = player.gameObject;

        if (_boardPlayers.Count == 4)
        {
            photonView.RPC("TurnHandler", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void TurnHandler()
    {
        switch (_turnOrder)
        {
            case TurnOrder.Player1:
                _boardPlayers[0].StartTurn();
                _boardPlayers[0].transform.GetChild(0).GetComponent<CinemachineCamera>().Priority = 1;
                break;
            case TurnOrder.Player2:
                _boardPlayers[1].StartTurn();
                _boardPlayers[1].transform.GetChild(0).GetComponent<CinemachineCamera>().Priority = 1;
                break;
            case TurnOrder.Player3:
                _boardPlayers[2].StartTurn();
                _boardPlayers[2].transform.GetChild(0).GetComponent<CinemachineCamera>().Priority = 1;
                break;
            case TurnOrder.Player4:
                _boardPlayers[3].StartTurn();
                _boardPlayers[3].transform.GetChild(0).GetComponent<CinemachineCamera>().Priority = 1;
                break;
            default:
                break;
        }
    }

    [PunRPC]
    public void LandOnTile(string tileName, string playerName)
    {
        Tiles tile = GameObject.Find(tileName)?.GetComponent<Tiles>();
        BoardPlayers player = GameObject.Find(playerName)?.GetComponent<BoardPlayers>();

        if (tile == null || player == null)
        {
            Debug.LogError($"Failed to resolve Tile ({tileName}) or Player ({playerName})!");
            return;
        }
        
        tile.LandOnTile(player);
    }

    [PunRPC]
    public void ArrangePlayers(string tileName, string playerName, bool add = false)
    {
        Tiles tile = GameObject.Find(tileName)?.GetComponent<Tiles>();
        BoardPlayers player = GameObject.Find(playerName)?.GetComponent<BoardPlayers>();

        if (tile == null || player == null)
        {
            Debug.LogError($"Failed to resolve Tile ({tileName}) or Player ({playerName})!");
            return;
        }

        tile.ArrangePlayers(player, add);
    }
    
    [PunRPC]
    public void NextTurn()
    {
        foreach(BoardPlayers player in _boardPlayers)
        {
            player.transform.GetChild(0).GetComponent<CinemachineCamera>().Priority = 0;
        }

        if (_turnOrder != TurnOrder.Player4)
        {
            _turnOrder = (TurnOrder)(int)_turnOrder + 1;
            TurnHandler();
        }
        else
        {
            MinigameDecider();
        }
    }

    [PunRPC]
    public void OpenMap(string playerName)
    {
        _lastPlayer = GameObject.Find(playerName)?.GetComponent<BoardPlayers>();

        _lastPlayer.transform.GetChild(0).GetComponent<CinemachineCamera>().Priority = 0;
    }

    [PunRPC]
    public void CloseMap()
    {
        _lastPlayer.transform.GetChild(0).GetComponent<CinemachineCamera>().Priority = 1;
    }

    private void MinigameDecider()
    {
        print("Idfk man just go play a minigame already");
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.LoadLevel("Minigame 1");
            RestartOrder();
        }
    }

    [PunRPC]
    public void RestartOrder()
    {
        _turnOrder = TurnOrder.Player1;
        TurnHandler();
    }

    public void AddCoins(BoardPlayers player, int amount)
    {
        for (int i = 0; i < _boardPlayers.Count; i++)
        {
            if (player == _boardPlayers[i])
            {
                print(player.name + " gained " + amount + " obols!");
                _playerData[i].Obols += amount;
                break;
            }
        }
    }

}
