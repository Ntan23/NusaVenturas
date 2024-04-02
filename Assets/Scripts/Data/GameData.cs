[System.Serializable]
public class GameData
{
    public int levelUnlocked;
    public int highScore;
    public int recipeCount;
    public SerializableDictionary<string,bool> recipeCollected;

    public GameData()
    {
        levelUnlocked = 1;
        highScore = 0;
        recipeCollected = new SerializableDictionary<string,bool>();
    }
}
