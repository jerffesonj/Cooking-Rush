using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private float maxTime = 120;
    private float currentTime;
    private int currentTimeInt;
    private int points;

    private bool gameStarted;

    private FoodScriptableObject currentFood;

    [SerializeField] private List<FoodScriptableObject> foodScripts;

    [SerializeField] private List<FoodLocationScript> plates;

    #region Events
    public delegate void OnNewFoodSelected(FoodScriptableObject food);
    public static event OnNewFoodSelected onNewFood;
    
    public delegate void OnGameStarted();
    public static event OnGameStarted onGameStarted;
    public static event OnGameStarted onGameEnded;

    public delegate void OnTimeChanged(int points);
    public static event OnTimeChanged onTimeChanged;

    public delegate void OnFoodCorrect(int points);
    public static event OnFoodCorrect onFoodCorrect;  
    
    public delegate void FoodCorrect();
    public static event FoodCorrect foodCorrect;

    public delegate void OnFoodIncorrect(int points);
    public static event OnFoodIncorrect onFoodIncorrect;

    public delegate void FoodIncorrect();
    public static event FoodIncorrect foodIncorrect;

    #endregion
    
    void Start()
    {
        FoodLocationScript.onFoodPlaced += CheckIngredientsOnPlate;

        currentTime  = maxTime;
        currentTimeInt = (int)currentTime;
        
        onFoodCorrect?.Invoke(points);
        
        StartGame();
    }

    private void OnDisable()
    {
        FoodLocationScript.onFoodPlaced -= CheckIngredientsOnPlate;
    }

    private void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        onGameStarted?.Invoke();
        yield return new WaitForSeconds(6.5f);
        gameStarted = true;
        SelectNewFood();
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        if (gameStarted)
        {
            currentTime -= Time.deltaTime;
            currentTimeInt = (int)currentTime;

            onTimeChanged?.Invoke(currentTimeInt);

            if (currentTime <= 0)
            {
                gameStarted = false;
                onGameEnded?.Invoke();
            }
        }
    }

    private void SelectNewFood()
    {
        int randomIndex = Random.Range(0, foodScripts.Count);
        currentFood = foodScripts[randomIndex];
        onNewFood?.Invoke(currentFood);
    }
    private void AddPoints(int value)
    {
        points += value;
        if (points <= 0)
            points = 0;
    }

    private IEnumerator RemoveFoodEnum()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < plates.Count; i++)
        {
            plates[i].RemoveFood();
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void CheckIngredientsOnPlate()
    {
        List<FoodScriptableObject> ingredients = new List<FoodScriptableObject>();

        for (int i = 0; i < currentFood.ingredients.Count; i++)
        {
            ingredients.Add(currentFood.ingredients[i]);
        }

        for (int i = 0; i < plates.Count; i++)
        {
            if (plates[i].currentFood == null)
            {
                return;
            }

            if (ingredients.Contains(plates[i].currentFood))
            {
                ingredients.Remove(plates[i].currentFood);
            }
        }

        if (ingredients.Count <=0)
        {
            AddPoints(100);
            onFoodCorrect?.Invoke(points);
            foodCorrect?.Invoke();
        }
        else
        {
            AddPoints(-100);
            onFoodIncorrect?.Invoke(points);
            foodIncorrect?.Invoke();
        }

        StartCoroutine(RemoveFoodEnum());
        
        SelectNewFood();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
