using UnityEngine;

public class IntersectionTile : Tiles
{
    [SerializeField] private GameObject m_arrowLeft;
    [SerializeField] private GameObject m_arrowRight;

    [SerializeField] private Transform m_leftTile;
    [SerializeField] private Transform m_rightTile;

    public override Transform GetNextTile(int selectedArrow = 0, BoardPlayers player = null)
    {
        if (selectedArrow == 1)
        {
            return m_rightTile;
        }
        else
        {
            return m_leftTile;
        }
    }

    public void StartSelectingArrows(BoardPlayers player)
    {
        player.StartDirectionSelection();
        SelectLeftArrow();
    }

    public void SelectLeftArrow()
    {
        m_arrowLeft.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 1f);
        m_arrowRight.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.5f);
    }

    public void SelectRightArrow()
    {
        m_arrowRight.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 1f);
        m_arrowLeft.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.5f);
    }
}
