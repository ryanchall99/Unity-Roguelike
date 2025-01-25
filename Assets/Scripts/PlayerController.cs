using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputSystem_Actions inputActions;
    InputAction moveAction;

    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    private void OnEnable() 
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.Enable();
        } 
    }

    private void OnDisable() 
    {
        inputActions.Player.Disable();    
    }

    private void Start() 
    {
        moveAction = inputActions.Player.Move;
    }

    private void Update() 
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (move.y > 0)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (move.y < 0)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (move.x > 0)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (move.x < 0)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved && moveAction.WasPressedThisFrame())
        {
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            // Check to see if cell data is valid & is a passable cell
            if (cellData != null && cellData.passable)
            {
                // Increase Game Tick by 1
                GameManager.Instance.turnManager.Tick();
                
                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);    
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    cellData.ContainedObject.PlayerEntered();
                }
            }
        }

    }

    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        // Move Player to right world position
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell);
    }
}
