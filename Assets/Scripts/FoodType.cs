using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodType : MonoBehaviour
{
    public enum FoodName
    {
        Bread,
        Tomato,
        Meat,
        Cheese,
        Bacon
    }
    public FoodName Name;
}
