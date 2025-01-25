using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] int foodAmount;
    public override void PlayerEntered()
    {
        GameManager.Instance.ChangeFood(foodAmount);
        Destroy(gameObject);
    }
}
