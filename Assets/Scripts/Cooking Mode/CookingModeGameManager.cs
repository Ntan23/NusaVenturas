using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CookingModeGameManager : MonoBehaviour, IData
{
    #region Singleton
    public static CookingModeGameManager instance;
    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    #region Variables
    [SerializeField] private bool endlessMode;
    [SerializeField] private FoodSO[] foods;
    private List<FoodSO> availableFoods;
    private int randomIndex;
    private bool collected;
    private bool isStarted;
    [Header("Trial")]
    private float coinCount; 
    private float initialTimeForTrial;
    private int levelIndex;
    private bool isCompleted;

    [Header("For Order")]
    [SerializeField] private TextMeshProUGUI orderName;
    [SerializeField] private Image orderFoodImage;
    [SerializeField] private TextMeshProUGUI orderFoodOrigin;
    [SerializeField] private TextMeshProUGUI ingredients;
    [SerializeField] private Image orderFoodIngredients;
    [SerializeField] private TextMeshProUGUI orderFoodPriceText;
    private int currentOrderID;
    private FoodSO currentFoodOrder;

    [Header("For Ingredient")]
    [SerializeField] private Transform addedIngredientParent;
    private GameObject addedIngredient;
    private GameObject spawnedIngredient;
    [SerializeField] private Transform ingredientSpawnPosition;
    private int ingredientCount;
    [SerializeField] private int maxIngredient;

    [Header("Cooking Indicator")]
    [SerializeField] private GameObject cookingIndicator;
    private Animator cookingAnimator;

    [Header("Others")]
    [SerializeField] private GameObject cookButton;
    [SerializeField] private Image timerBar;
    [SerializeField] private GameObject blackScreen;
    private float currentTime;
    private float cookingSpeed;

    [Header("Endless")]
    [SerializeField] private float maxTime;
    [SerializeField] private TextMeshProUGUI currentProfitText;
    [SerializeField] private TextMeshProUGUI highestProfitText;
    [Header("Trial")]
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private GameObject completedFoodImage;
    private float highestProfit;
    private float currentProfit;
    [Header("Others")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Settings settings;
    private bool canBePressed = true;
    private bool isPaused;
    private bool isFirstTime = true;
    private AudioManager am;
    #endregion

    void Start()
    {
        am = AudioManager.instance;

        cookingAnimator = cookingIndicator.GetComponent<Animator>();

        if(endlessMode)
        {
            highestProfitText.text = "Highest Profits : " + highestProfit.ToString("0.00");
            currentProfitText.text = "Current Profits : " + currentProfit.ToString("0.00");
        }

        if(!endlessMode)
        {
            maxTime = initialTimeForTrial;
            coinCountText.text = coinCount.ToString("0.00");
        }
        
        currentTime = maxTime;
        
        for(int i = 0; i < foods.Length; i++) 
        {
            if(foods[i].isUnlocked) availableFoods.Add(foods[i]);
            if(!foods[i].isUnlocked) continue;
        }

        GenerateNewOrder();
        
        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
        {
            blackScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;

            isStarted = true;
        });

        am.Play("Cooking");
    }

    public void LoadData(GameData gameData) 
    {
        if(endlessMode) this.highestProfit = gameData.highestProfit;
        if(!endlessMode) 
        {
            this.coinCount = gameData.coinCount;
            this.levelIndex = gameData.levelIndex;
            this.initialTimeForTrial = gameData.initialTimeForTrial;
        }

        this.cookingSpeed = gameData.cookingSpeed;

        this.availableFoods = new List<FoodSO>();
        
        for(int i = 0; i < foods.Length; i++)
        {
            gameData.recipeCollected.TryGetValue(foods[i].foodRecipeID,out collected);
            
            if(collected) foods[i].isUnlocked = true;
            if(!collected) foods[i].isUnlocked = false;
        }
    }
    
    public void SaveData(GameData gameData) 
    {
        if(endlessMode) gameData.highestProfit = this.highestProfit;
        if(!endlessMode) 
        {
            if(this.isCompleted)
            {
                gameData.coinCount = this.coinCount;
                gameData.initialTimeForTrial = this.initialTimeForTrial;
                gameData.isInTrialMode[levelIndex - 1] = false;
                gameData.fromTrialMode[levelIndex - 1] = true;
            } 
            else if(!this.isCompleted) gameData.isInTrialMode[levelIndex - 1] = true;
        }
    }
    
    void Update()
    {
        if(!isStarted) return;

        if(!isPaused) currentTime -= Time.deltaTime;

        UpdateTimerBar();

        if(currentTime <= 0)
        {
            CompleteGame();
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!canBePressed) return; 

            if(!isPaused) Pause();
            if(isPaused) 
            {
                if(settings != null)
                {
                    if(!settings.GetIsOpen()) Resume();
                    if(settings.GetIsOpen()) return;
                }

                if(settings == null) Resume();
            }
        }
    }

    public void GoToMainMenu() 
    {
        am.Stop("Cooking");
        Time.timeScale = 1.0f;

        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("Main Menu"));
    }

    private void Pause()
    {
        am.Stop("Cooking");
        canBePressed = false;

        LeanTween.value(pauseMenuUI, UpdatePauseMenuUIAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
        {
            pauseMenuUI.GetComponent<CanvasGroup>().interactable = true;
            pauseMenuUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
            isPaused = true;
            canBePressed = true;

            Time.timeScale = 0.0f;
        });
    }

    public void Resume()
    {
        canBePressed = false;
        Time.timeScale = 1.0f;
        am.Play("Cooking");

        LeanTween.value(pauseMenuUI, UpdatePauseMenuUIAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
        {
            pauseMenuUI.GetComponent<CanvasGroup>().interactable = false;
            pauseMenuUI.GetComponent<CanvasGroup>().blocksRaycasts = false;

            isPaused = false;
            canBePressed = true;
        });
    }

    private void UpdatePauseMenuUIAlpha(float alpha) => pauseMenuUI.GetComponent<CanvasGroup>().alpha = alpha;

    public void GenerateNewOrder()
    {
        if(availableFoods != null)
        {
            if(!endlessMode) 
            {
                if(!isFirstTime) randomIndex = Random.Range(0, availableFoods.Count);
                
                if(isFirstTime) 
                {
                    randomIndex = levelIndex - 1;

                    isFirstTime = false;
                }
            }

            if(endlessMode) randomIndex = Random.Range(0, availableFoods.Count);

            currentFoodOrder = availableFoods[randomIndex];

            orderName.text = currentFoodOrder.foodName;
            orderFoodOrigin.text = currentFoodOrder.foodOrigin;
            orderFoodImage.sprite = currentFoodOrder.foodSpriteWithFrame;
            //ingredients.text = currentFoodOrder.foodIngredients;
            orderFoodIngredients.sprite = currentFoodOrder.foodIngredientsSprite;
            orderFoodPriceText.text = "+ " + currentFoodOrder.foodPrice.ToString("0.00");
        }
    }

    public void AddIngredient(IngredientSO ingredientSO)
    {
        if(ingredientCount < maxIngredient)
        {
            StartCoroutine(PlayDropSFX());
            currentOrderID += ingredientSO.ingredientID;

            addedIngredient = Instantiate(ingredientSO.ingredientSprite, addedIngredientParent);
            spawnedIngredient = Instantiate(ingredientSO.spawnedIngredient, ingredientSpawnPosition);
            Destroy(spawnedIngredient, 0.6f);

            ingredientCount++;
        }
    }

    IEnumerator PlayDropSFX()
    {
        am.Play("Drop");
        yield return new WaitForSeconds(0.6f);
        am.Play("WaterSplash");
    }

    public void CookFood()
    {
        if(currentOrderID != currentFoodOrder.foodID) TrashFood();
        else if(currentOrderID == currentFoodOrder.foodID) StartCoroutine(Cook());
    }
    
    public void TrashFood()
    {
        if(ingredientCount > 0)
        {
            am.Play("Trash");

            if(endlessMode)
            {
                currentProfit --;
                if(currentProfit < 0) currentProfit = 0;
                
                currentProfitText.text = "Current Profits : " + currentProfit.ToString("0.00");
            }

            if(!endlessMode) 
            {
                coinCount--;
                if(coinCount < 0) coinCount = 0;

                coinCountText.text = coinCount.ToString("0.00");
            }

            ResetIngredients();
        }
    }

    private void ResetIngredients()
    {
        for(int i = ingredientCount - 1; i >= 0; i--) Destroy(addedIngredientParent.GetChild(i).gameObject);

        currentOrderID = 0;
        ingredientCount = 0;
    }

    private void UpdateTimerBar()
    {
        timerBar.fillAmount = currentTime / maxTime;

        if(timerBar.fillAmount <= 0.5f && timerBar.fillAmount > 0.1f) timerBar.color = Color.yellow;
        if(timerBar.fillAmount <= 0.1f) timerBar.color = Color.red;
    }

    IEnumerator Cook()
    {
        ResetIngredients();
        cookButton.SetActive(false);
        LeanTween.value(cookingIndicator, UpdateCookingIndicatorAlpha, 0.0f, 1.0f, 0.2f).setOnComplete(() => 
        {
            cookingAnimator.enabled = true;
            cookingIndicator.GetComponent<CanvasGroup>().blocksRaycasts = true;
        });
        yield return new WaitForSeconds(cookingSpeed);
        am.Play("FinishCooking");
        LeanTween.value(cookingIndicator, UpdateCookingIndicatorAlpha, 1.0f, 0.0f, 0.2f).setOnComplete(() => 
        {
            cookingAnimator.enabled = false;
            cookingIndicator.GetComponent<CanvasGroup>().blocksRaycasts = false;
        });

        if(endlessMode)
        {
            currentProfit += currentFoodOrder.foodPrice;
            currentProfitText.text = "Current Profits : " + currentProfit.ToString("0.00");

            if(currentProfit > highestProfit) 
            {
                highestProfitText.text = "Highest Profits : " + currentProfit.ToString("0.00");

                highestProfit = currentProfit;
            }
        }

        if(!endlessMode) 
        {
            coinCount += currentFoodOrder.foodPrice;

            coinCountText.text = coinCount.ToString("0.00");
        }

        completedFoodImage.GetComponent<SpriteRenderer>().sprite = currentFoodOrder.foodSpriteWithoutFrame;

        LeanTween.scale(completedFoodImage, Vector3.one, 0.3f).setOnComplete(() => 
        {
            StartCoroutine(ShowFood());
        });
    }

    private void CompleteGame()
    {
        am.Stop("Cooking");
        
        if(!endlessMode)
        {
            isCompleted = true;
            initialTimeForTrial += 8.0f;

            SceneManager.LoadScene("Level" + levelIndex.ToString());
        }
    }

    private void UpdateCookingIndicatorAlpha(float alpha) => cookingIndicator.GetComponent<CanvasGroup>().alpha = alpha;
    private void UpdateBlackscreenAlpha(float alpha) => blackScreen.GetComponent<CanvasGroup>().alpha = alpha;
    
    IEnumerator ShowFood()
    {
        yield return new WaitForSeconds(0.2f);
        LeanTween.moveX(completedFoodImage, -12.0f, 0.5f).setOnComplete(() => 
        {
            GenerateNewOrder();
            completedFoodImage.transform.localScale = Vector3.zero;
            completedFoodImage.transform.position = Vector3.zero;

            cookButton.SetActive(true);
            
            if(endlessMode) currentTime += 10;
            
            UpdateTimerBar();
        });
    }
}
