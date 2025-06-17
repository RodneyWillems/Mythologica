using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardPlayers : MonoBehaviour
{
    public float WaitTime;

    private bool m_myTurn;
    private PlayerInput m_playerInput;

    private void Start()
    {
        StartCoroutine(Wait());
        m_playerInput = GetComponent<PlayerInput>();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(WaitTime);
        BoardgameManager.Instance.AddPlayer(this);
    }

    public void StartTurn()
    {
        print("It's my turn! (" + name + ")");
        m_myTurn = true;
    }

    public void AddCoins(int amount)
    {
        BoardgameManager.Instance.AddCoins(this, amount);
        // Play happy animation
    }

    public void RemoveCoins(int amount)
    {
        BoardgameManager.Instance.AddCoins(this, -amount);
        // Play sad animation
    }

    public void StartDirectionSelection()
    {
        m_playerInput.currentActionMap = null;
    }



    private void OnGUI()
    {
        if (m_myTurn)
        {
            if (GUI.Button(new Rect(50, 100, 200, 75), name + "'s Turn"))
            {
                BoardgameManager.Instance.NextTurn();
                m_myTurn = false;
            }
        }
    }
}
