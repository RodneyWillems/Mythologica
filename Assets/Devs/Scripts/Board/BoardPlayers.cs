using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class BoardPlayers : MonoBehaviour
{
    public float WaitTime;

    // Turn logic
    private bool _myTurn;
    private int _movesLeft;
    private Coroutine _movingRoutine;
    private Transform _nextTilePosition;
    private Tiles _lastTile;
    private IntersectionTile _intersection;

    // Misc
    private Board _controls;

    private void OnEnable()
    {
        _controls = new();
        _controls.Enable();

        _controls.Intersection.SelectRight.performed += SelectRightArrow;
        _controls.Intersection.SelectLeft.performed += SelectLeftArrow;
        _controls.Intersection.SelectOption.performed += SelectIntersectionOption;

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
        _lastTile = FindAnyObjectByType<StartingTile>();
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

    public void UseDice()
    {
        int randomDiceNumber = Random.Range(1, 7);
        _movesLeft = randomDiceNumber;
        print(randomDiceNumber);
        print("Starting moving!");
        MoveLogic();
    }

    private void MoveLogic()
    {
        if (_movesLeft > 0 && _movingRoutine == null)
        {
            _movingRoutine = StartCoroutine(Move());
        }
        else if (_movesLeft <= 0)
        {
            print("Done moving!");
            // _myTurn = false;
            Collider[] otherPlayers = Physics.OverlapSphere(transform.position, 2);
            BoardgameManager.Instance.NextTurn();
        }
    }

    private IEnumerator Move(bool useIntersection = false)
    {
        if (useIntersection)
        {
            _nextTilePosition = _intersection.GetNextTile(this);
        }
        else
        {
            _nextTilePosition = _lastTile.GetNextTile(this);
        }
        if (_nextTilePosition != null)
        {
            _lastTile = _nextTilePosition.GetComponent<Tiles>();
            while (Vector3.Distance(transform.position, _nextTilePosition.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _nextTilePosition.position, 0.5f);
                yield return new WaitForEndOfFrame();
            }
            _movingRoutine = null;
            _movesLeft--;
            MoveLogic();
            yield return null;
        }
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

    private void SelectIntersectionOption(InputAction.CallbackContext context)
    {
        _movingRoutine = StartCoroutine(Move(true));
    }

    private void OnGUI()
    {
        if (_myTurn)
        {
            if (GUI.Button(new Rect(50, 100, 200, 75), name + "'s Turn"))
            {
                UseDice();
                _myTurn = false;
            }
        }
    }
}
