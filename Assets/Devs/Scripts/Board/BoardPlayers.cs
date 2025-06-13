using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardPlayers : MonoBehaviour
{
    public float WaitTime;

    private IntersectionTile _intersection;
    private Tiles _lastTile;

    private bool _myTurn;
    private Board _controls;

    private void OnEnable()
    {
        _controls = new();
        _controls.Enable();

        _controls.Intersection.SelectRight.performed += SelectRightArrow;
        _controls.Intersection.SelectLeft.performed += SelectLeftArrow;
        _controls.Intersection.SelectOption.performed += SelecIntersectionOption;

        _controls.Turn.Select.performed += SelectTurnOption;
        _controls.Turn.Down.performed += NextOption;
        _controls.Turn.Up.performed += PreviousOption;
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Start()
    {
        StartCoroutine(Wait());
        _controls.Disable();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(WaitTime);
        BoardgameManager.Instance.AddPlayer(this);
    }

    public void StartTurn()
    {
        print("It's my turn! (" + name + ")");
        _myTurn = true;
        _controls.Turn.Enable();
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

    private void SelectTurnOption(InputAction.CallbackContext context)
    {
        
    }

    private void NextOption(InputAction.CallbackContext context)
    {
        
    }

    private void PreviousOption(InputAction.CallbackContext context)
    {
        
    }

    public void StartDirectionSelection(IntersectionTile intersection)
    {
        _controls.Intersection.Enable();

        _intersection = intersection;
    }

    private void SelectRightArrow(InputAction.CallbackContext context)
    {
        _intersection.SelectRightArrow();
    }

    private void SelectLeftArrow(InputAction.CallbackContext context)
    {
        _intersection.SelectLeftArrow();
    }

    private void SelecIntersectionOption(InputAction.CallbackContext context)
    {
        _intersection.GetNextTile();
    }

    private void OnGUI()
    {
        if (_myTurn)
        {
            if (GUI.Button(new Rect(50, 100, 200, 75), name + "'s Turn"))
            {
                BoardgameManager.Instance.NextTurn();
                _myTurn = false;
            }
        }
    }
}
