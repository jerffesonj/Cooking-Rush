using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLocationScript : MonoBehaviour
{
    public List<GameObject> foodIngredients = new List<GameObject>();

    public FoodScriptableObject currentFood;

    public delegate void OnFoodPlaced();
    public static event OnFoodPlaced onFoodPlaced;
   
    public void ActivateFood(FoodScriptableObject foodPlaced)
    {
        foreach (GameObject food in foodIngredients)
        {
            if (foodPlaced.foodType == food.GetComponent<FoodType>().Name)
            {
                food.SetActive(true);
            }
            else
            {
                food.SetActive(false);
            }
        }
        currentFood = foodPlaced;

        onFoodPlaced?.Invoke();
    }

    public void RemoveFood()
    {
        foreach (GameObject food in foodIngredients)
        {
            food.SetActive(false);
            currentFood = null;
        }
    }
}
