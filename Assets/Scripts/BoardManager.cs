using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool passable;
    }

    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;
    [SerializeField] Tile[] groundTiles;
    [SerializeField] Tile[] wallTiles;
    [SerializeField] PlayerController player;

    private Tilemap m_Tilemap;
    private Grid m_Grid;
    private CellData[,] m_BoardData;

    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        m_BoardData = new CellData[boardWidth, boardHeight]; // Initializing with correct array size for all each cell data to be stored

        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData(); // Creating each cell data

                if (x == 0 || y == 0 || x == boardWidth - 1 || y == boardHeight - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    m_BoardData[x, y].passable = false;
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    m_BoardData[x, y].passable = true;
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= boardWidth || cellIndex.y < 0 || cellIndex.y > boardHeight)
        {
            // Stops searches of cells which don't exist
            return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }
}
