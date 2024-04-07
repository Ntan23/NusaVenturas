using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour, IData
{
    [SerializeField] private string id;
    private bool collected = false;
    private bool flag;
    private int collectedRecipeCount;
    [SerializeField] private Transform playerTransform;
    private float posX, posY, posZ;

    public void LoadData(GameData gameData) 
    {
        gameData.recipeCollected.TryGetValue(id,out collected);

        if(collected) 
        {
            this.gameObject.SetActive(false);
            flag = true;
        }

        this.collectedRecipeCount = gameData.collectedRecipeCount;
    }

    public void SaveData(GameData gameData) 
    {
        if(gameData.recipeCollected.ContainsKey(id)) gameData.recipeCollected.Remove(id);
        
        gameData.recipeCollected.Add(id,collected);
   
        gameData.collectedRecipeCount = this.collectedRecipeCount;
        
        if(!flag) 
        {
            gameData.posX = this.posX;
            gameData.posY = this.posY;
            gameData.posZ = this.posZ;    
        }
    }

    public void PickUpRecipe()
    {
        collected = true;
        collectedRecipeCount++;
        this.gameObject.SetActive(false);

        this.posX = playerTransform.localPosition.x;
        this.posY = playerTransform.localPosition.y;
        this.posZ = playerTransform.localPosition.z;
    }
}
