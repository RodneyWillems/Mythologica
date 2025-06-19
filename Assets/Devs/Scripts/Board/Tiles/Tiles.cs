using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Photon.Realtime;
using Photon.Pun;

[Serializable]
public class Tiles : MonoBehaviourPun
{
    [SerializeField] protected Transform _nextTile;
    [SerializeField] protected int _coinsAdded;
    [SerializeField] protected int _moveAway;

    [SerializeField] protected List<BoardPlayers> _playersOnTile = new();

    public virtual void LandOnTile(BoardPlayers player)
    {
        player.AddCoins(_coinsAdded);
        StartArrangingPlayers(player);
    }

    protected void StartArrangingPlayers(BoardPlayers player)
    {
        string tileName = name;
        string playerName = player.name;
        BoardgameManager.Instance.photonView.RPC("ArrangePlayers", RpcTarget.AllBuffered, tileName, playerName);
    }

    public virtual void ArrangePlayers(BoardPlayers player)
    {
        if (_playersOnTile.Contains(player))
        {
            _playersOnTile.Remove(player);
        }
        else
        {
            _playersOnTile.Add(player);
        }

        for (int i = 0; i < _playersOnTile.Count; i++)
        {
            _playersOnTile[i].CorrectPosition(transform.position + Vector3.right * i * _moveAway);
        }
    }

    public virtual Transform GetNextTile(BoardPlayers player = null)
    {
        StartArrangingPlayers(player);
        return _nextTile;
    }
}
