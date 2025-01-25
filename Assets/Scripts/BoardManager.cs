using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool passable;
        public CellObject ContainedObject;
    }

    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;
    [SerializeField] Tile[] groundTiles;
    [SerializeField] Tile[] wallTiles;
    [SerializeField] FoodObject[] foodPrefabArray;
    [SerializeField] WallObject[] wallPrefabArray;
    [SerializeField] int minFood, maxFood;
    [SerializeField] int minWalls, maxWalls;
    [SerializeField] PlayerController player;

    private Tilemap m_Tilemap;
    private Grid m_Grid;
    private CellData[,] m_BoardData;
    private List<Vector2Int> m_EmptyCellsList;

    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
        // Initialize the list
        m_EmptyCellsList = new List<Vector2Int>();
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

                    // Passable cell so add to list
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        // Removing player space from list
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));
        GenerateWalls();
        GenerateFood();
    }

    private void GenerateWalls()
    {
        int wallCount = Random.Range(minWalls, maxWalls);
        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            WallObject newWall = Instantiate(wallPrefabArray[Random.Range(0, wallPrefabArray.Length)]);
            AddObject(newWall, coord);
        }
    }

    private void GenerateFood()
    {
        int foodCount = Random.Range(minFood, maxFood);
        for (int i = 0; i < foodCount; i++)
        {
            // Choosing random index from empty cell list
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex]; // Storing coord of cell chosen

            m_EmptyCellsList.RemoveAt(randomIndex); // Removing chosen coordinate from list (Prevents being chosen again)
            FoodObject newFood = Instantiate(foodPrefabArray[Random.Range(0, foodPrefabArray.Length)]); // Instantiate food prefab
            AddObject(newFood, coord);
        }
    }

    private void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y]; // Getting data from chosen cell coordinate
        obj.transform.position = CellToWorld(coord); // Setting position of object (Converting from cell to world space)
        data.ContainedObject = obj; // Updating the cells contained object data
        obj.Init(coord); // Calling base Init function
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

    // SETTER - Sets a specific tile
    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
}
