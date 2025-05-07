using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private Vector3 _correctDirection;

    private Vector3 _firstDirection;
    private Vector3 _targetPosition;
    private bool _move;
    private bool _mashing;

    private void Start()
    {
        _targetPosition = transform.position + (_correctDirection * 6);
        _firstDirection = _correctDirection;
    }

    public bool CheckMove(Vector3 direction, bool doneMashing = false)
    {
        if (direction == _correctDirection && !_mashing)
        {
            _move = true;
            return true;
        }
        else if (!_mashing)
        {
            _mashing = true;
            switch (Random.Range(0, 4))
            {
                case 0:
                    _correctDirection = Vector2.up;
                    break;
                case 1:
                    _correctDirection = Vector2.down;
                    break;
                case 2:
                    _correctDirection = Vector2.right;
                    break;
                case 3:
                    _correctDirection = Vector2.left;
                    break;
                default:
                    break;
            }
            return false;
        }
        else if (_mashing && direction == _correctDirection && !doneMashing)
        {
            return true;
        }
        else if (doneMashing) 
        {
            _move = true;
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (_move)
        {
            transform.position += _correctDirection / 20;
            if (Vector3.Distance(transform.position, _targetPosition) <= 1)
            {
                _move = false;
                Destroy(gameObject);
            }
        }
    }
}
