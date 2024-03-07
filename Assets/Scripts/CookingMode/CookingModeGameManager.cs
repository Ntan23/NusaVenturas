using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingModeGameManager : MonoBehaviour
{
    [SerializeField] private FoodSO[] foods;
    private int randomIndex;

    [Header("For Order")]
    [SerializeField] private TextMeshProUGUI orderName;
    [SerializeField] private Image orderFoodImage;
    [SerializeField] private TextMeshProUGUI orderFoodOrigin;
    [SerializeField] private TextMeshProUGUI ingredients;
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

    private int currentScore;
    private int score;

    void Start()
    {
        GenerateNewOrder();
    }

    public void GenerateNewOrder()
    {
        randomIndex = Random.Range(0, foods.Length);

        orderName.text = foods[randomIndex].foodName;
        orderFoodOrigin.text = foods[randomIndex].foodOrigin;
        orderFoodImage.sprite = foods[randomIndex].foodSprite;
        ingredients.text = foods[randomIndex].foodIngredients;

        currentFoodOrder = foods[randomIndex];
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
        for(int i = addedIngredientParent.childCount - 1; i >= 0; i--) Destroy(addedIngredientParent.GetChild(i).gameObject);

        currentOrderID = 0;
        ingredientCount = 0;
    }

    IEnumerator Cook()
    {
        Debug.Log("Cooking");
        TrashFood();
        cookButton.SetActive(false);
        yield return new WaitForSeconds(currentFoodOrder.cookTime);
        Debug.Log("Finish Cooking");
        currentScore += currentFoodOrder.foodScore;
        cookButton.SetActive(true);
        GenerateNewOrder();
    }
}
