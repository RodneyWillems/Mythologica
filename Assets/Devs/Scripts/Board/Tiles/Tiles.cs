using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Photon.Realtime;

[Serializable]
public class Tiles : MonoBehaviour
{
    [SerializeField] protected Transform _nextTile;
    [SerializeField] protected int _coinsAdded;
    [SerializeField] protected int _moveAway;

    [SerializeField] protected List<BoardPlayers> _playersOnTile = new();

    public virtual void LandOnTile(BoardPlayers player)
    {
        player.AddCoins(_coinsAdded);
        _playersOnTile.Add(player);
        ArrangePlayers();
    }

    protected virtual void ArrangePlayers()
    {
        for (int i = 0; i < _playersOnTile.Count; i++)
        {
            _playersOnTile[i].CorrectPosition(transform.position + Vector3.right * i * _moveAway);
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
