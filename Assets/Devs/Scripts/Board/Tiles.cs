using UnityEngine;

public class Tiles : MonoBehaviour
{
    [SerializeField] protected Transform m_nextTile;
    [SerializeField] protected int m_coinsAdded;

    public virtual void LandOnTile(BoardPlayers player)
    {
        player.AddCoins(m_coinsAdded);
    }

    public virtual Transform GetNextTile(int selectedArrow = 0, BoardPlayers player = null)
    {
        if (m_nextTile.GetComponent<IntersectionTile>() != null)
        {
            m_nextTile.GetComponent<IntersectionTile>().StartSelectingArrows(player);
        }
        return m_nextTile;
    }
}
