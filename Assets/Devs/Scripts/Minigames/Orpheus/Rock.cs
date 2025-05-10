using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rock : MonoBehaviour
{
    public IconPreset iconPreset; 
    
    [SerializeField] private Vector3 _correctDirection;
    [SerializeField] private MeshRenderer _iconRenderer;
    
    private Vector3 _targetPosition;
    private bool _mashing;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        RandomizeDirection();
    }
    public bool CheckMove(Vector3 direction, bool doneMashing = false)
    {
        // When the player presses the correct button the rock moves out of the way
        if (direction == _correctDirection && !_mashing)
        {
            StartCoroutine(Move());
            return true;
        }
        // When the player presses the wrong button the first time the mashing sequence begins
        else if (!_mashing)
        {
            _mashing = true;
            RandomizeDirection();
            _animator.SetTrigger("Mash");
            return false;
        }
        // When the player presses the correct button but they have to mash the rock moves a small amount
        else if (_mashing && direction == _correctDirection && !doneMashing)
        {
            transform.position += _correctDirection / 20;
            return true;
        }
        // When the player is done mashing the rock moves out of the way
        else if (doneMashing) 
        {
            StartCoroutine(Move());
            return true;
        }
        return false;
    }

    private IEnumerator Move()
    {
        // When the rock moves it destroys itself once it's out of the screen
        while (Vector3.Distance(transform.position, _targetPosition) > 0.5f)
        {
            transform.position += _correctDirection / 20;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private void RandomizeDirection()
    {
        // When the rock gets spawned OR the player presses the wrong button the rock gets a random direction and sets it's icon as the correct button
        switch (Random.Range(0, 4))
        {
            case 0:
                _correctDirection = Vector2.up;
                _iconRenderer.material.SetTexture("Base_Map", iconPreset.iconDict["up"]);
                break;
            case 1:
                _correctDirection = Vector2.down;
                _iconRenderer.material.SetTexture("Base_Map", iconPreset.iconDict["down"]);
                break;
            case 2:
                _correctDirection = Vector2.right;
                _iconRenderer.material.SetTexture("Base_Map", iconPreset.iconDict["right"]);
                break;
            case 3:
                _correctDirection = Vector2.left;
                _iconRenderer.material.SetTexture("Base_Map", iconPreset.iconDict["left"]);
                break;
            default:
                break;
        }
        _targetPosition = transform.position + (_correctDirection * 6);
    }
}
