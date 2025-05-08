using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Orpheus : MonoBehaviour
{
    #region Variables

    [SerializeField] private LayerMask _rockLayer;

    [Header("Failed")]
    [SerializeField] private int _failedInputsNeeded;

    private Rock _failedRock;
    private int _currentFailedInputs;
    private bool _failed;
    private Coroutine _failRoutine;

    // Moving
    private Coroutine _movingRoutine;

    #endregion

    #region Inputs

    public void MoveRock(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return; 
        }
        // When the player presses a button it checks with the next rock if it's the correct button or not
        Vector2 input = context.ReadValue<Vector2>();
        if (Physics.BoxCast(transform.position, (Vector3.up + Vector3.right) * 4, transform.forward, out RaycastHit hit, Quaternion.identity, 1))
        {
            if (hit.collider.GetComponent<Rock>().CheckMove(input) && !_failed)
            {
                // If the player presses the correct button they move on
                if (_movingRoutine == null)
                {
                    _movingRoutine = StartCoroutine(MoveForward());
                }
            }
            else if (hit.collider.GetComponent<Rock>().CheckMove(input) && _failed)
            {
                // After the player has failed if they press the correct button it helps to move the rock
                print("Right input for mashing!");
                _currentFailedInputs++;
            }
            else
            {
                // If the player presses the wrong button it starts the mashing sequence
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
        // When the player gets the correct input they move forward
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
        // When the player fails an input they have to start spamming the new correct key before continuing
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
}
