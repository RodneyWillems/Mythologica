using UnityEngine;

public class IntersectionTile : Tiles
{
    [SerializeField] private MeshRenderer _arrowLeft;
    [SerializeField] private MeshRenderer _arrowRight;

    [SerializeField] private Transform _leftTile;
    [SerializeField] private Transform _rightTile;

    private int _selectedArrow;
    private bool _firstTime = true;

    public override Transform GetNextTile(BoardPlayers player = null)
    {
        if (_firstTime)
        {
            StartSelectingArrows(player);
            _firstTime = false;
            return null;
        }
        else if (_selectedArrow == 1)
        {
            _firstTime = true;
            _arrowLeft.gameObject.SetActive(false);
            _arrowRight.gameObject.SetActive(false);
            return _rightTile;
        }
        else
        {
            _firstTime = true;
            _arrowLeft.gameObject.SetActive(false);
            _arrowRight.gameObject.SetActive(false);
            return _leftTile;
        }
    }

    public void StartSelectingArrows(BoardPlayers player)
    {
        _arrowLeft.gameObject.SetActive(true);
        _arrowRight.gameObject.SetActive(true);
        player.StartDirectionSelection(this);
        SelectLeftArrow();
    }

    public void SelectLeftArrow()
    {
        _selectedArrow = 0;
        _arrowLeft.material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        _arrowRight.material.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    }

    public void SelectRightArrow()
    {
        _selectedArrow = 1;
        _arrowRight.material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        _arrowLeft.material.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    }
}
