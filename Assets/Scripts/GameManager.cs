using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] BoardManager boardManager;
    [SerializeField] PlayerController player;

    private int m_FoodCount = 100;

    public TurnManager turnManager { get; private set; }

    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() 
    {
        turnManager = new TurnManager();
        turnManager.OnTick += OnTurnHappen;
        
        // Board & Player Initialization
        boardManager.Init();
        player.Spawn(boardManager, new Vector2Int(1, 1));   
    }

    private void OnTurnHappen()
    {
        m_FoodCount -= 1;
        Debug.Log("Food Count: " + m_FoodCount);
    }
}
