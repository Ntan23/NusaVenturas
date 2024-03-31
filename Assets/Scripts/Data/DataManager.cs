using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [Header("File Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useAESEncryption;

    private GameData gameData;
    private List<IData> dataObjects;
    private DataHandler dataHandler;

    public static DataManager instance { get; private set; }

    private void Awake() 
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new DataHandler(Application.persistentDataPath,fileName,useAESEncryption);
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene,LoadSceneMode mode) 
    {
        this.dataObjects = FindAllDataObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene) => SaveGame();
    
    public void NewGame() 
    {
        this.gameData = new GameData();
        PlayerPrefs.SetString("??",Security.EncryptKey(Security.RandomKeyGenerator()));
        PlayerPrefs.SetString("!!",Security.EncryptIV(Security.RandomIVGenerator()));
    }

    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();
        
        // if no data can be loaded, initialize to a new game
        if (this.gameData == null) NewGame();
        
        // push the loaded data to all other scripts that need it
        foreach (IData dataObj in dataObjects) dataObj.LoadData(gameData);
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IData dataObj in dataObjects) dataObj.SaveData(gameData);
        
        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit() => SaveGame();
    
    private List<IData> FindAllDataObjects() 
    {
        IEnumerable<IData> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IData>();

        return new List<IData>(dataObjects);
    }
}
