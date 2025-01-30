using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] int foodAmount;
    [SerializeField] bool isSoda;
    public override void PlayerEntered()
    {
        GameManager.Instance.ChangeFood(foodAmount);
        
        if (isSoda)
        {
            AudioManager.Instance.PlaySFX("Soda");
        }
        else
        {
            AudioManager.Instance.PlaySFX("Fruit");
        }

        Destroy(gameObject);
    }
}
