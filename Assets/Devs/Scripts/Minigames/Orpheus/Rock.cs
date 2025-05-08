using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rock : MonoBehaviour
{
    [HideInInspector] public IconPreset iconPreset; 
    
    [SerializeField] private Vector3 _correctDirection;
    [SerializeField] private MeshRenderer _iconRenderer;
    
    private Vector3 _targetPosition;
    private bool _mashing;

    private void Start()
    {
        RandomizeDirection();
    }
    public bool CheckMove(Vector3 direction, bool doneMashing = false)
    {
        // When input correct move
        if (direction == _correctDirection && !_mashing)
        {
            // _targetPosition = transform.position + (_correctDirection * 6);
            StartCoroutine(Move());
            return true;
        }
        // When input false first time no move start mash
        else if (!_mashing)
        {
            _mashing = true;
            RandomizeDirection();
            return false;
        }
        // When input right but mash no move but correct
        else if (_mashing && direction == _correctDirection && !doneMashing)
        {
            return true;
        }
        // When done mash go move
        else if (doneMashing) 
        {
            StartCoroutine(Move());
            return true;
        }
        return false;
    }

    private IEnumerator Move()
    {
        // When rock move rock move out of screen and poof gone
        while (Vector3.Distance(transform.position, _targetPosition) > 0.5f)
        {
            transform.position += _correctDirection / 20;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private void RandomizeDirection()
    {
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
