using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    [SerializeField] Tile[] ObstacleTilesArray;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        GameManager.Instance.boardManager.SetCellTile(cell, ObstacleTilesArray[Random.Range(0, ObstacleTilesArray.Length)]);
    }
}
