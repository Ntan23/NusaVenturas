using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingModeGameManager : MonoBehaviour
{
    [SerializeField] private FoodSO[] foods;
    private int randomIndex;
    private int tempID;

    [Header("For Order")]
    [SerializeField] private TextMeshProUGUI orderName;
    [SerializeField] private Image orderFoodImage;
    [SerializeField] private TextMeshProUGUI foodOrigin;
    [SerializeField] private TextMeshProUGUI ingredients;
    private int currentOrderID;

    [Header("For Ingredient")]
    [SerializeField] private Transform addedIngredientParent;
    private GameObject addedIngredient;
    private GameObject spawnedIngredient;
    [SerializeField] private Transform ingredientSpawnPosition;
    private int ingredientCount;
    [SerializeField] private int maxIngredient;

    void Start()
    {
        GenerateNewOrder();
    }

    public void GenerateNewOrder()
    {
        randomIndex = Random.Range(0, foods.Length);

        orderName.text = foods[randomIndex].foodName;
        orderFoodImage.sprite = foods[randomIndex].foodSprite;
        ingredients.text = foods[randomIndex].foodIngredients;

        currentOrderID = foods[randomIndex].foodID;
    }

    public void AddIngredient(IngredientSO ingredientSO)
    {
        if(ingredientCount < maxIngredient)
        {
            tempID = ingredientSO.ingredientID;

            addedIngredient = Instantiate(ingredientSO.ingredientSprite, addedIngredientParent);
            spawnedIngredient = Instantiate(ingredientSO.spawnedIngredient, ingredientSpawnPosition);
            Destroy(spawnedIngredient, 0.6f);

            ingredientCount++;
        }
    }
}
