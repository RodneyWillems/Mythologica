using UnityEngine;

public class StartingTile : Tiles
{
    [SerializeField] private BoardPlayers _addToStart;

    private void Start()
    {
        StartArrangingPlayers(_addToStart, true);
    }

    public override void LandOnTile(BoardPlayers player)
    {
        StartArrangingPlayers(player);
    }
}
