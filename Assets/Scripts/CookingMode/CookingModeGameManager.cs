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
    [SerializeField] private TextMeshProUGUI ingredients;
    private int currentOrderID;

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
}
