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
    public float posX, posY, posZ;
    public int levelIndex;
    public int collectedRecipeCount;
    public SerializableDictionary<string,bool> recipeCollected;
    public bool[] isInTrialMode;
    public bool[] fromTrialMode;
    #endregion

    public GameData()
    {
        coinCount = 0;
        initialTimeForTrial = 30.0f;
        cookingSpeed = 3.0f;
        highestProfit = 0;
        
        levelUnlocked = 1;
        levelIndex = 1;
        posX = 0.0f;
        posY = 0.0f;
        posZ = 0.0f;
        collectedRecipeCount = 0;
        recipeCollected = new SerializableDictionary<string,bool>();
        isInTrialMode = new bool[6];
        fromTrialMode = new bool[6];
    }
}
