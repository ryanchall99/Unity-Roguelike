using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Board & Player")]
    [SerializeField] PlayerController player;
    [SerializeField] int m_FoodCount = 100;

    [Header("UI")]
    [SerializeField] UIDocument UIDoc;

    private Label m_FoodLabel;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;

    private int m_CurrentLevel = 1;

    public TurnManager turnManager { get; private set; }
    public BoardManager boardManager;

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
        
        NewLevel();

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage"); // Child of m_GameOverPanel

        UpdateFoodLabel();
        m_GameOverPanel.style.visibility = Visibility.Hidden;
    }

    public void NewLevel()
    {
        boardManager.ClearBoard(); // Clear existing board
        boardManager.Init(); // Initialize new board
        player.Spawn(boardManager, new Vector2Int(1, 1)); // Spawn player at bottom left corner

        m_CurrentLevel++; // Increment level count by 1
    }

    public void ChangeFood(int amount)
    {
        m_FoodCount += amount;
        UpdateFoodLabel();

        if (m_FoodCount <= 0)
        {
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurrentLevel + " levels";
        }
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
