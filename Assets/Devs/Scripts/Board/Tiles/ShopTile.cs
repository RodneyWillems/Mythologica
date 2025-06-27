using UnityEngine;

public class ShopTile : Tiles
{
    private BoardPlayers _currentPlayer;

    public override void LandOnTile(BoardPlayers player)
    {
        // Shop 
        base.LandOnTile(player);
    }

    private void UseShop(BoardPlayers player)
    {
        _currentPlayer = player;

    }
}
