using UnityEngine;

public class EnemyObject : CellObject
{
    [SerializeField] int health = 3;
    [SerializeField] int damage = 3;
    
    private int m_CurrentHealth;

    private void Awake() 
    {
        GameManager.Instance.turnManager.OnTick += OnTurnHappen;   
    }

    private void OnDestroy() 
    {
        GameManager.Instance.turnManager.OnTick -= OnTurnHappen;    
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_CurrentHealth = health;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHealth -= 1; // Reduce Health by 1

        if (m_CurrentHealth > 0)
        {
            return false;
        }

        Destroy(gameObject); // Destroy GameObject
        return true;
    }

    bool MoveTo(Vector2Int coord)
    {
        BoardManager board = GameManager.Instance.boardManager;
        var targetCell = board.GetCellData(coord);

        // Target is not null || target not passable || target has already got contained object
        if (targetCell == null || !targetCell.passable || targetCell.ContainedObject != null)
        {
            return false;
        }

        // Remove from current cell
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        // Add to next cell
        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    private void OnTurnHappen()
    {
        Vector2Int playerCell = GameManager.Instance.GetPlayerController().GetCurrentCell();

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            // Player is adjacent to enemy
            GameManager.Instance.ChangeFood(-damage);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveXAxis(xDist))
                {
                    // If unable to move in X & not attack
                    // Try to move along Y
                    TryMoveYAxis(yDist);
                }
            }
            else
            {
                if (!TryMoveYAxis(yDist))
                {
                    TryMoveXAxis(xDist);
                }
            }
        }
    }

    private bool TryMoveXAxis(int xDist)
    {
        // Player to right
        if (xDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.right);
        }

        // Player to left
        return MoveTo(m_Cell + Vector2Int.left);
    }

    private bool TryMoveYAxis(int yDist)
    {
        // Player above
        if (yDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.up);
        }

        // Player Below
        return MoveTo(m_Cell + Vector2Int.down);
    }
}
