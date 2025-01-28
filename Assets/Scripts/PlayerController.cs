using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputSystem_Actions inputActions;
    InputAction moveAction;
    InputAction enterAction;

    [SerializeField] float movementSpeed;

    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private Vector3 m_MoveTarget;
    private bool m_IsGameOver = false;
    private bool m_IsMoving;

    // Animation
    private Animator m_Animator;

    private void Awake() 
    {
        m_Animator = GetComponent<Animator>();    
    }

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
        enterAction = inputActions.Player.Enter;
    }

    public void Init()
    {
        m_IsGameOver = false;
        m_IsMoving = false;
        inputActions.Player.Move.Enable();
    }

    private void Update() 
    {   
        if (m_IsGameOver)
        {
            if (enterAction.WasPressedThisFrame())
            {
                GameManager.Instance.StartNewGame();
            }
            
            return;
        }

        if (m_IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, movementSpeed * Time.deltaTime);
            if (transform.position == m_MoveTarget)
            {
                m_IsMoving = false;
                var cellData = m_Board.GetCellData(m_CellPosition);
                if (cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEntered();
                }
            }

            return; // Early return to stop movement before reaching tile
        }
        
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
                    MoveTo(newCellTarget, false);    
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget, false);
                }
            }
        }

    }

    public void MoveTo(Vector2Int cell, bool immediate)
    {
        m_CellPosition = cell;

        if (immediate)
        {
            m_IsMoving = false;
            transform.position = m_Board.CellToWorld(m_CellPosition);
        }
        else
        {
            m_IsMoving = true;
            m_MoveTarget = m_Board.CellToWorld(m_CellPosition);
        }
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell, true);
    }

    public Vector2Int GetCurrentCell()
    {
        return m_CellPosition;
    }

    public void GameOver()
    {
        m_IsGameOver = true;
        inputActions.Player.Move.Disable();
    }

    public void Attack()
    {
        m_Animator.SetTrigger(AnimatorNames.Attack);
    }
}
