using System.Collections;
using System.Collections.Generic;
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
    private PlayerData[] m_playerData;

    private List<BoardPlayers> m_boardPlayers = new();

    private TurnOrder m_turnOrder;

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
        m_turnOrder = TurnOrder.Player1;
        m_playerData = new PlayerData[4];
        for (int i = 0; i < 4; i++)
        {
            m_playerData[i].Obols = 20;
        }
    }

    public void AddPlayer(BoardPlayers player)
    {
        m_boardPlayers.Add(player);
        m_playerData[m_boardPlayers.Count].PlayerName = player.name;
        m_playerData[m_boardPlayers.Count].PlayerObject = player.gameObject;
    }

    private void TurnHandler()
    {
        switch (m_turnOrder)
        {
            case TurnOrder.Player1:
                m_boardPlayers[0].StartTurn();
                break;
            case TurnOrder.Player2:
                m_boardPlayers[1].StartTurn();
                break;
            case TurnOrder.Player3:
                m_boardPlayers[2].StartTurn();
                break;
            case TurnOrder.Player4:
                m_boardPlayers[3].StartTurn();
                break;
            default:
                break;
        }
    }

    public void NextTurn()
    {
        if (m_turnOrder != TurnOrder.Player4)
        {
            m_turnOrder = (TurnOrder)(int)m_turnOrder + 1;
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
                m_turnOrder = TurnOrder.Player1;
                TurnHandler();
            }
        }
    }

    public void AddCoins(BoardPlayers player, int amount)
    {
        for (int i = 0; i < m_boardPlayers.Count; i++)
        {
            if (player == m_boardPlayers[i])
            {
                m_playerData[i].Obols += amount;
                break;
            }
        }
    }

}
