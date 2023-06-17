using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Food", order = 0)]
public class FoodScriptableObject : ScriptableObject
{
    public string foodName;

    public FoodType.FoodName foodType;

    public Sprite icon;
    public List<FoodScriptableObject> ingredients;
}
