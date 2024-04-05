[System.Serializable]
public class GameData
{
    #region CookingMode
    public float coinCount;
    public float initialTimeForTrial;
    public float cookingSpeed;
    public int highestProfit;
    #endregion
    
    #region PlatformerMode
    public int levelUnlocked;
    public int collectedRecipeCount;
    public SerializableDictionary<string,bool> recipeCollected;
    #endregion

    public GameData()
    {
        coinCount = 0;
        initialTimeForTrial = 30.0f;
        cookingSpeed = 3.0f;
        highestProfit = 0;
        
        levelUnlocked = 1;
        collectedRecipeCount = 0;
        recipeCollected = new SerializableDictionary<string,bool>();
    }
}
