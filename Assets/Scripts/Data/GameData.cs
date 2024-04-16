[System.Serializable]
public class GameData
{
    #region ForCookingMode
    public float coinCount;
    public float initialTimeForTrial;
    public float cookingSpeed;
    public int cookingSpeedLevel;
    public float cookingSpeedUpgradeCost;
    public int highestProfit;
    #endregion
    
    #region ForPlatformerMode
    public float posX, posY, posZ;
    public float healthCount;
    public float savedHealth;
    public int healthLevel;
    public float healthUpgradeCost;
    public float immunityTime;
    public int immunityLevel;
    public float immunityTimeUpgradeCost;
    public float jumpPower;
    public int jumpLevel;
    public float jumpPowerUpgradeCost;
    public int levelUnlocked;
    public int levelIndex;
    public int collectedRecipeCount;
    public SerializableDictionary<string,bool> recipeCollected;
    public bool[] isInTrialMode;
    public bool[] fromTrialMode;
    #endregion

    #region ForSetttings
    public int fullscreenIndicator;
    public float bgmVolume;
    public float sfxVolume;
    public float masterVolume;
    #endregion

    public GameData()
    {
        #region CookingMode
        coinCount = 0;
        initialTimeForTrial = 30.0f;
        cookingSpeed = 3.0f;
        cookingSpeedLevel = 1;
        cookingSpeedUpgradeCost = 3.0f;
        highestProfit = 0;
        #endregion
        
        #region PlatformerMode
        posX = 0.0f;
        posY = 0.0f;
        posZ = 0.0f;
        healthCount = 2.0f;
        savedHealth = 0.0f;
        healthLevel = 1;
        healthUpgradeCost = 4.0f;
        immunityTime = 1.0f;
        immunityLevel = 1;
        immunityTimeUpgradeCost = 5.0f;
        jumpPower = 6.5f;
        jumpLevel = 1;
        jumpPowerUpgradeCost = 2.5f;
        levelUnlocked = 1;
        levelIndex = 1;
        collectedRecipeCount = 0;
        recipeCollected = new SerializableDictionary<string,bool>();
        isInTrialMode = new bool[6];
        fromTrialMode = new bool[6];
        #endregion

        #region Settings
        fullscreenIndicator = 1;
        bgmVolume = 0.5f;
        sfxVolume = 1.0f;
        masterVolume = 1.0f;
        #endregion
    }
}
