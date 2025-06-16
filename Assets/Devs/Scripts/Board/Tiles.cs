using UnityEngine;

public class Tiles : MonoBehaviour
{
    [SerializeField] protected Transform _nextTile;
    [SerializeField] protected int _coinsAdded;

    public virtual void LandOnTile(BoardPlayers player)
    {
        player.AddCoins(_coinsAdded);
    }

    public virtual Transform GetNextTile(BoardPlayers player = null)
    {
        if (_nextTile.GetComponent<IntersectionTile>() != null)
        {
            _nextTile.GetComponent<IntersectionTile>().StartSelectingArrows(player);
        }
        return _nextTile;
    }
}
