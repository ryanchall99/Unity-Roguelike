using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Board & Player")]
    [SerializeField] BoardManager boardManager;
    [SerializeField] PlayerController player;

    [Header("UI")]
    [SerializeField] UIDocument UIDoc;

    private int m_FoodCount = 100;
    private Label m_FoodLabel;

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

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        UpdateFoodLabel();
    }

    public void ChangeFood(int amount)
    {
        m_FoodCount += amount;
        UpdateFoodLabel();
    }

    private void OnTurnHappen()
    {
        ChangeFood(-1);
        UpdateFoodLabel();
    }

    private void UpdateFoodLabel()
    {
        m_FoodLabel.text = "Food : " + m_FoodCount;
    }

}
