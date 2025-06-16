using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public enum TurnOrder
{
    Player1,
    Player2, 
    Player3, 
    Player4,
}

public class BoardgameManager : MonoBehaviour
{
    public static BoardgameManager Instance { get; private set; }

    [SerializeField]
    private PlayerData[] _playerData;

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
    }

    private void Start()
    {
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
        m_minigameTime = true;
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(50, 200, 200, 75), "Start Turn Handling"))
        {
            TurnHandler();
        }
        if (m_minigameTime)
        {
            if (GUI.Button(new Rect(50, 100, 200, 75), "Restart order"))
            {
                m_minigameTime = false;
                _turnOrder = TurnOrder.Player1;
                TurnHandler();
            }
        }
    }

    public void AddCoins(BoardPlayers player, int amount)
    {
        for (int i = 0; i < _boardPlayers.Count; i++)
        {
            if (player == _boardPlayers[i])
            {
                _playerData[i].Obols += amount;
                break;
            }
        }
    }

}
