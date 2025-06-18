using UnityEngine;
using System.Collections.Generic;

public class Tiles : MonoBehaviour
{
    [SerializeField] protected Transform _nextTile;
    [SerializeField] protected int _coinsAdded;
    [SerializeField] protected int _moveAway;

    private List<BoardPlayers> _playersOnTile = new();

    public virtual void LandOnTile(BoardPlayers player)
    {
        player.AddCoins(_coinsAdded);
        _playersOnTile.Add(player);
        if (_playersOnTile.Count > 1)
        {
            print("THERE'S OTHERS");
            player.CorrectPosition(transform.position + Vector3.right * _playersOnTile.Count * _moveAway);
        }
    }

    public virtual Transform GetNextTile(BoardPlayers player = null)
    {
        if (_playersOnTile.Contains(player))
        {
            _playersOnTile.Remove(player);
            foreach (BoardPlayers fixPlayer in _playersOnTile)
            {
                fixPlayer.CorrectPosition(transform.position + Vector3.right * (_playersOnTile.Count - 1) * _moveAway);
            }
        }
        return _nextTile;
    }
}
