using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Orpheus : MonoBehaviour
{
    [SerializeField] private LayerMask m_rockLayer;

    private Minigames _controls;

    #region Setup

    private void OnEnable()
    {
        _controls = new Minigames();
        _controls.Orpheus.Moverock.performed += MoveRock;
    }

    void Start()
    {

    }

    #endregion

    #region Inputs

    private void MoveRock(InputAction.CallbackContext context)
    {
        Vector2 input = _controls.Orpheus.Moverock.ReadValue<Vector2>();
        if (Physics.Raycast(transform.position, transform.forward, 0.5f, m_rockLayer))
        {

        }
    }

    #endregion

    public void StartMinigame()
    {
        _controls.Orpheus.Enable();
    }

    void Update()
    {
        
    }
}
