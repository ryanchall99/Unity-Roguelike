using UnityEngine;

public class EnemyObject : CellObject
{
    [SerializeField] int health = 3;
    
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

    private void OnTurnHappen()
    {

    }
}
