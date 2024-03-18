using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingModeGameManager : MonoBehaviour
{
    #region Singleton
    public static CookingModeGameManager instance;
    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    #region Variables
    [SerializeField] private FoodSO[] foods;
    private int randomIndex;

    [Header("For Order")]
    [SerializeField] private TextMeshProUGUI orderName;
    [SerializeField] private Image orderFoodImage;
    [SerializeField] private TextMeshProUGUI orderFoodOrigin;
    [SerializeField] private TextMeshProUGUI ingredients;
    [SerializeField] private TextMeshProUGUI orderScoreText;
    private int currentOrderID;
    private FoodSO currentFoodOrder;

    [Header("For Ingredient")]
    [SerializeField] private Transform addedIngredientParent;
    private GameObject addedIngredient;
    private GameObject spawnedIngredient;
    [SerializeField] private Transform ingredientSpawnPosition;
    private int ingredientCount;
    [SerializeField] private int maxIngredient;

    [Header("Others")]
    [SerializeField] private GameObject cookButton;
    [SerializeField] private Image timerBar;
    [SerializeField] private float maxTime;
    private float currentTime;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    private int highscore;

    private int currentScore;
    private int score;
    #endregion

    void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        currentTime = maxTime;

        scoreText.text = "Score : " + currentScore.ToString();
        highscoreText.text = "Highscore : " + highscore.ToString();

        GenerateNewOrder();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        UpdateTimerBar();

        if(currentTime <= 0)
        {
            Debug.LogError("You Have Complete The Game");
            return;
        }
    }

    public void GenerateNewOrder()
    {
        randomIndex = Random.Range(0, foods.Length);

        currentFoodOrder = foods[randomIndex];

        orderName.text = currentFoodOrder.foodName;
        orderFoodOrigin.text = currentFoodOrder.foodOrigin;
        orderFoodImage.sprite = currentFoodOrder.foodSprite;
        ingredients.text = currentFoodOrder.foodIngredients;
        orderScoreText.text = "Food Score : " + currentFoodOrder.foodScore.ToString();

        Debug.Log(currentFoodOrder.foodID);
    }

    public void AddIngredient(IngredientSO ingredientSO)
    {
        if(ingredientCount < maxIngredient)
        {
            currentOrderID += ingredientSO.ingredientID;
            Debug.Log(currentOrderID);

            addedIngredient = Instantiate(ingredientSO.ingredientSprite, addedIngredientParent);
            spawnedIngredient = Instantiate(ingredientSO.spawnedIngredient, ingredientSpawnPosition);
            Destroy(spawnedIngredient, 0.6f);

            ingredientCount++;
        }
    }

    public void CookFood()
    {
        if(currentOrderID != currentFoodOrder.foodID) Debug.Log("Wrong Ingredients");
        else if(currentOrderID == currentFoodOrder.foodID) StartCoroutine(Cook());
    }
    
    public void TrashFood()
    {
        if(ingredientCount > 0)
        {
            currentScore -= ingredientCount * 50;
            if(currentScore <= 0) currentScore = 0;

            scoreText.text = "Score : " + currentScore.ToString();

            for(int i = ingredientCount - 1; i >= 0; i--) Destroy(addedIngredientParent.GetChild(i).gameObject);

            currentOrderID = 0;
            ingredientCount = 0;
        }
    }

    private void UpdateTimerBar()
    {
        timerBar.fillAmount = currentTime / maxTime;

        if(timerBar.fillAmount <= 0.5f && timerBar.fillAmount > 0.1f) timerBar.color = Color.yellow;
        if(timerBar.fillAmount <= 0.1f) timerBar.color = Color.red;
    }

    IEnumerator Cook()
    {
        Debug.Log("Cooking");
        TrashFood();
        cookButton.SetActive(false);
        yield return new WaitForSeconds(currentFoodOrder.cookTime);
        Debug.Log("Finish Cooking");
        currentScore += currentFoodOrder.foodScore;
        scoreText.text = "Score : " + currentScore.ToString();

        if(currentScore > highscore) 
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
            highscoreText.text = "Highscore : " + currentScore.ToString();

            highscore = currentScore;
        }

        cookButton.SetActive(true);
        GenerateNewOrder();
    }
}
