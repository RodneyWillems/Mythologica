using UnityEngine;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Photon.Realtime;

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
        ArrangePlayers();
    }

    protected virtual void ArrangePlayers()
    {
        foreach (BoardPlayers player in _playersOnTile)
        {
            player.CorrectPosition(transform.position + Vector3.right * (_playersOnTile.Count - 1) * _moveAway);
        }
    }

    public virtual Transform GetNextTile(BoardPlayers player = null)
    {
        if (_playersOnTile.Contains(player))
        {
            _playersOnTile.Remove(player);
            ArrangePlayers();
        }
        return _nextTile;
    }
}
