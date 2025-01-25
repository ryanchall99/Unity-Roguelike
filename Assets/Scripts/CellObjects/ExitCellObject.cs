using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    [SerializeField] Tile ExitTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManager.Instance.boardManager.SetCellTile(m_Cell, ExitTile);
    }

    public override void PlayerEntered()
    {
        Debug.Log("Exit Reached!");
    }
}
