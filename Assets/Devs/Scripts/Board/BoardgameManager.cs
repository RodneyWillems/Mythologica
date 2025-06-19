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

    [SerializeField]
    private PlayerData[] _playerData;
    
    [SerializeField]
    private GameObject[] _playerObjects;

    private List<BoardPlayers> _boardPlayers = new();

    private TurnOrder _turnOrder;

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

        string playerModelName = DataManager.Instance.MyPlayerClass.Playermodel.Model.name + "Player";
        foreach (GameObject playerObject in _playerObjects)
        {
            if (playerObject.name == playerModelName)
            {
                playerObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

    // GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerName, Vector3.zero, Quaternion.identity, 0);
        //
        // StartingTile startingTile = FindAnyObjectByType<StartingTile>();
        // spawnedPlayer.transform.position = startingTile.transform.position;
        // startingTile.LandOnTile(spawnedPlayer.GetComponent<BoardPlayers>());
        
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

    private bool m_minigameTime;

    private void MinigameDecider()
    {
        print("Idfk man just go play a minigame already");
        PhotonNetwork.LoadLevel("Minigame 1");
        m_minigameTime = true;
    }

    [PunRPC]
    public void RestartOrder()
    {
        m_minigameTime = false;
        _turnOrder = TurnOrder.Player1;
        TurnHandler();
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(50, 200, 200, 75), "Start Turn Handling"))
        {
            photonView.RPC("TurnHandler", RpcTarget.AllBuffered);
            // TurnHandler();
        }
        if (m_minigameTime)
        {
            if (GUI.Button(new Rect(50, 100, 200, 75), "Restart order"))
            {
                photonView.RPC("RestartOrder", RpcTarget.AllBuffered);
                // RestartOrder(); 
            }
        }
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
