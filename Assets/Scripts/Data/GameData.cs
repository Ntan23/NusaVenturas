[System.Serializable]
public class GameData
{
    public int levelsUnlocked;
    public int highScore;
    public SerializableDictionary<string,bool> recipeCollected;

    public GameData()
    {
        levelsUnlocked = 1;
        highScore = 0;
        recipeCollected = new SerializableDictionary<string,bool>();
    }
}
