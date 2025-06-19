using System;
using System.Collections;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class BoardPlayers : MonoBehaviourPun
{
    [SerializeField] public float WaitTime;

    // Turn logic
    [SerializeField] private GameObject _turnButtons;

    [SerializeField] private bool _myTurn;
    [SerializeField] private int _movesLeft;
    private Coroutine _movingRoutine;
    [SerializeField] private Transform _nextTilePosition;
    [SerializeField] private Tiles _lastTile;
    [SerializeField] private IntersectionTile _intersection;

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
        StartCoroutine(Wait());
        // BoardgameManager.Instance.AddPlayer(this);
        _controls.Disable();
        _lastTile = FindAnyObjectByType<StartingTile>();

        // if (photonView.IsMine)
        // {
        //     _turnButtons = GameObject.Find("Player " + DataManager.Instance.MyPlayerClass.Id);
        //     _turnButtons.SetActive(false);
        //     
        //     _turnButtons.GetComponent<Button>().onClick.AddListener(UseDice);
        // }
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
        if (!photonView.IsMine) { return; }
        int randomDiceNumber = Random.Range(1, 7);
        _movesLeft = randomDiceNumber;
        photonView.RPC("DisableButtons", RpcTarget.AllBuffered);
        MoveLogic();
    }

    [PunRPC]
    private void DisableButtons()
    {
        _turnButtons.SetActive(false);
    }
    
    private void MoveLogic()
    {
        if (_movesLeft > 0 && _movingRoutine == null)
        {
            _movingRoutine = StartCoroutine(Move());
        }
        else if (_movesLeft <= 0)
        {
            string tileName = _lastTile.name;
            string playerName = name;
            
            BoardgameManager.Instance.photonView.RPC("LandOnTile", RpcTarget.AllBuffered, tileName, playerName);
            BoardgameManager.Instance.photonView.RPC("NextTurn", RpcTarget.AllBuffered);
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
                transform.position = Vector3.MoveTowards(transform.position, _nextTilePosition.position, 25 * Time.deltaTime);
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
            transform.position = Vector3.MoveTowards(transform.position, position, 25 * Time.deltaTime);
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
