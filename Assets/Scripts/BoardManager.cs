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

    private Tilemap m_Tilemap;
    private CellData[,] m_BoardData;

    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
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
}
