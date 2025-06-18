using System;
using System.Collections;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardPlayers : MonoBehaviourPun
{
    public float WaitTime;

    // Turn logic
    [SerializeField] private GameObject _turnButtons;

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
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Start()
    {
        // StartCoroutine(Wait());
        BoardgameManager.Instance.AddPlayer(this);
        _controls.Disable();
        _lastTile = FindAnyObjectByType<StartingTile>();

        if (photonView.IsMine)
        {
            _turnButtons = GameObject.Find("Player " + DataManager.Instance.MyPlayerClass.Id);
        }
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
        _turnButtons.SetActive(true);
        _turnButtons.transform.GetChild(0).GetComponent<Button>().Select();
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

    public void UseDice()
    {
        int randomDiceNumber = Random.Range(1, 7);
        _movesLeft = randomDiceNumber;
        _turnButtons.SetActive(false);
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
            _lastTile.LandOnTile(this);
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
                transform.position = Vector3.MoveTowards(transform.position, _nextTilePosition.position, 0.3f);
                yield return new WaitForEndOfFrame();
            }
            _movingRoutine = null;
            _movesLeft--;
            MoveLogic();
            yield return null;
        }
    }

    public void CorrectPosition(Vector3 position)
    {
        _movingRoutine = StartCoroutine(CorrectMyself(position));
    }

    private IEnumerator CorrectMyself(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 0.3f);
            yield return new WaitForEndOfFrame();
        }
        _movingRoutine = null;
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

        _controls.Intersection.Disable();
    }
}
