using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] BoardManager boardManager;
    [SerializeField] PlayerController player;

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
        
        // Board & Player Initialization
        boardManager.Init();
        player.Spawn(boardManager, new Vector2Int(1, 1));   
    }
}
