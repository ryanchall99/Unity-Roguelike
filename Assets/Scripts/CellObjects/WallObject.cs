using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    [SerializeField] Tile[] ObstacleTilesArray;
    [SerializeField] int maxHealth;

    private int m_HealthPoints;
    private Tile m_OriginalTile; 

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_HealthPoints = maxHealth;
        m_OriginalTile = GameManager.Instance.boardManager.GetCellTile(cell); // Caching original tile before change 

        GameManager.Instance.boardManager.SetCellTile(cell, ObstacleTilesArray[Random.Range(0, ObstacleTilesArray.Length)]);
    }

    public override bool PlayerWantsToEnter()
    {
        m_HealthPoints -= 1;

        if (m_HealthPoints > 0)
        {
            return false;
        }

        GameManager.Instance.boardManager.SetCellTile(m_Cell, m_OriginalTile); // Reset to original tile
        Destroy(gameObject); // Destroy Prefab
        return true;
    }
}
