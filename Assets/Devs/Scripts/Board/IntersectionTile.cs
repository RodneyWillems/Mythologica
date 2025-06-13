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
            print(_rightTile.name);
            return _rightTile;
        }
        else
        {
            _firstTime = true;
            print(_leftTile.name);
            return _leftTile;
        }
    }

    public void StartSelectingArrows(BoardPlayers player)
    {
        player.StartDirectionSelection(this);
        SelectLeftArrow();
    }

    public void SelectLeftArrow()
    {
        _selectedArrow = 0;
        _arrowLeft.material.color = new Color(1, 1, 1, 1f);
        _arrowRight.material.color = new Color(1, 1, 1, 0.2f);
        print("Selected left!");
    }

    public void SelectRightArrow()
    {
        _selectedArrow = 1;
        _arrowRight.material.color = new Color(1, 1, 1, 1f);
        _arrowLeft.material.color = new Color(1, 1, 1, 0.2f);
        print("Selected right!");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 300, 200, 75), "Test Intersection"))
        {
            GetNextTile(FindFirstObjectByType<BoardPlayers>());
        }
        if (GUI.Button(new Rect(50, 400, 200, 75), "Select intersection"))
        {
            GetNextTile();
        }
    }
}
