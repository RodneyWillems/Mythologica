using UnityEngine;

public class NegativeTile : Tiles
{
    public override void LandOnTile(BoardPlayers player)
    {
        player.RemoveCoins(_coinsAdded);
    }
}
