using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour, IData
{
    [SerializeField] private string id;
    private bool collected = false;
    private int collectedRecipeCount;
    
    public void LoadData(GameData gameData) 
    {
        gameData.recipeCollected.TryGetValue(id,out collected);

        if(collected) this.gameObject.SetActive(false);

        this.collectedRecipeCount = gameData.collectedRecipeCount;
    }

    public void SaveData(GameData gameData) 
    {
        if(gameData.recipeCollected.ContainsKey(id)) gameData.recipeCollected.Remove(id);
        
        gameData.recipeCollected.Add(id,collected);

        gameData.collectedRecipeCount = this.collectedRecipeCount;
    }

    public void PickUpRecipe()
    {
        collected = true;
        collectedRecipeCount++;
        this.gameObject.SetActive(false);
    }
}
