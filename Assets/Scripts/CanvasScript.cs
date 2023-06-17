using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [Header("Food Information")]
    [SerializeField] private Image foodIcon;
    [SerializeField] private TMP_Text foodName;
    [SerializeField] private Image[] ingredientsIcons;

    [Space]

    [Header("Game Feedback")]
    [SerializeField] private TMP_Text startGameText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject foodInformationObject;

    [Space]

    [Header("Food Feedback")]
    [SerializeField] private GameObject correctAnswer;
    [SerializeField] private GameObject correctAnswerPoints;
    [SerializeField] private GameObject incorrectAnwser;
    [SerializeField] private GameObject incorrectAnwserPoints;

    [Header("End Game")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text endText;

    private void Awake()
    {
        GameController.onGameStarted += AnimateStartText;
        GameController.onTimeChanged += ChangeTime;
        GameController.onGameEnded += ShowEndScreen;

        GameController.onNewFood += SetFoodInformation;

        GameController.onFoodCorrect += UpdatePoints;
        GameController.foodCorrect += CorrectAnswer;
        
        GameController.onFoodIncorrect += UpdatePoints;
        GameController.foodIncorrect += IncorrectAnswer;
    }
   
    private void OnDisable()
    {
        GameController.onGameStarted -= AnimateStartText;
        GameController.onTimeChanged -= ChangeTime;
        GameController.onGameEnded -= ShowEndScreen;

        GameController.onNewFood -= SetFoodInformation;
        
        GameController.onFoodCorrect -= UpdatePoints;
        GameController.foodCorrect -= CorrectAnswer;

        GameController.onFoodIncorrect -= UpdatePoints;
        GameController.foodIncorrect -= IncorrectAnswer;

    }

    private void Update()
    {
    }

    void ChangeTime(int value)
    {
        timeText.text = value.ToString("D2");
    }

    public void SetFoodInformation(FoodScriptableObject food)
    {
        foodIcon.sprite = food.icon;
        foodName.text = food.foodName;

        if (food.ingredients.Count <= 0)
        {
            for (int i = 0; i < ingredientsIcons.Length; i++)
            {
                ingredientsIcons[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < ingredientsIcons.Length; i++)
            {
                ingredientsIcons[i].sprite = food.ingredients[i].icon;
            }
        }
        foodInformationObject.SetActive(false);
        foodInformationObject.SetActive(true);
    }

    public void AnimateStartText()
    {
        StartCoroutine(StartTextAnimation());
    }

    IEnumerator StartTextAnimation()
    {
        startGameText.text = "Make as much food as you can under 120 seconds";
        yield return new WaitForSeconds(2.5f);

        for (int i = 3; i >= 1; i--)
        {
            startGameText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        startGameText.text = "Cook!";
        yield return new WaitForSeconds(1f);

        startGameText.transform.parent.gameObject.SetActive(false);
    }

    public void UpdatePoints(int value)
    {
        pointsText.text = value.ToString();
    }

    public void CorrectAnswer()
    {
        correctAnswer.SetActive(true);
        correctAnswerPoints.SetActive(true);
    }
    public void IncorrectAnswer()
    {
        incorrectAnwser.SetActive(true);
        incorrectAnwserPoints.SetActive(true);
    }

    public void ShowEndScreen()
    {
        endScreen.SetActive(true);
        endText.text = "You've made " + pointsText.text + " points";
    }
}
