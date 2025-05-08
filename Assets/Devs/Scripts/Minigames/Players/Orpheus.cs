using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Orpheus : MonoBehaviour
{
    [SerializeField] private LayerMask _rockLayer;

    [Header("Failed")]
    [SerializeField] private int _failedInputsNeeded;

    private Rock _failedRock;
    private int _currentFailedInputs;
    private bool _failed;
    private Coroutine _failRoutine;

    // Moving
    private Coroutine _movingRoutine;
    private Minigames _controls;

    // Misc
    private MinigameManager _minigameManager;

    #region Setup

    private void OnEnable()
    {
        _controls = new Minigames();
        _controls.Orpheus.Moverock.performed += MoveRock;
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void Start()
    {
        StartMinigame();
        _minigameManager = FindFirstObjectByType<MinigameManager>();
    }

    #endregion

    #region Inputs

    private void MoveRock(InputAction.CallbackContext context)
    {
        Vector2 input = _controls.Orpheus.Moverock.ReadValue<Vector2>();
        if (Physics.BoxCast(transform.position, (Vector3.up + Vector3.right) * 4, transform.forward, out RaycastHit hit, Quaternion.identity, 1))
        {
            if (hit.collider.GetComponent<Rock>().CheckMove(input) && !_failed)
            {
                if (_movingRoutine == null)
                {
                    _movingRoutine = StartCoroutine(MoveForward());
                }
            }
            else if (hit.collider.GetComponent<Rock>().CheckMove(input) && _failed)
            {
                print("Right input for mashing!");
                _currentFailedInputs++;
            }
            else
            {
                print("Wrong input start mashing!!!");
                if (_failRoutine == null)
                {
                    _failedRock = hit.collider.GetComponent<Rock>();
                    _failed = true;
                    _failRoutine = StartCoroutine(Failed());
                }
            }
        }
    }

    private IEnumerator MoveForward()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 targetPosition = transform.position + transform.forward;
        while (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
        {
            transform.position += transform.forward / 8;
            yield return new WaitForEndOfFrame();
        }
        _movingRoutine = null;
    }

    private IEnumerator Failed()
    {
        while (_currentFailedInputs < _failedInputsNeeded)
        {
            _failed = true;
            yield return new WaitForEndOfFrame();
        }

        if (_movingRoutine == null)
        {
            _movingRoutine = StartCoroutine(MoveForward());
        }

        _currentFailedInputs = 0;
        _failedRock.CheckMove(Vector2.up, true);
        _failed = false;
        _failRoutine = null;
        yield return null;
    }

    #endregion

    public void StartMinigame()
    {
        _controls.Orpheus.Enable();
    }

    public void EndMinigame()
    {
        _controls.Orpheus.Disable();
    }

    void Update()
    {
        
    }
}
