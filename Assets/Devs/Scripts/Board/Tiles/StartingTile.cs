using UnityEngine;

public class StartingTile : Tiles
{
    public override void LandOnTile(BoardPlayers player)
    {
        ArrangePlayers(player);
    }
}
