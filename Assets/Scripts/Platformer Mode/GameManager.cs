using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IData
{
    #region Singleton
    public static GameManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    #region Variables
    private GameObject player;
    private Vector3 intialPosition;
    private bool canControl;
    private bool isComplete;
    private bool fromTrialMode;
    private float posX, posY, posZ;
    [SerializeField] private int levelIndex;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject recipeWindow;
    [SerializeField] private GameObject nextLevelDoor;
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(!fromTrialMode) nextLevelDoor.SetActive(false);
        else if(fromTrialMode) 
        {
            player.transform.localPosition = new Vector3(posX, posY, posZ);
            nextLevelDoor.SetActive(true);
        }
        
        intialPosition = player.transform.localPosition;

        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() => canControl = true);
    }

    public void LoadData(GameData gameData)
    {
        this.fromTrialMode = gameData.fromTrialMode[this.levelIndex - 1];
        
        Debug.Log(this.fromTrialMode);
        Debug.Log(new Vector3(gameData.posX, gameData.posY, gameData.posZ));

        if(gameData.fromTrialMode[this.levelIndex - 1]) 
        {
            this.posX = gameData.posX;
            this.posY = gameData.posY;
            this.posZ = gameData.posZ;
        }
    }

    public void SaveData(GameData gameData) 
    {
        gameData.levelIndex = this.levelIndex;

        if(this.levelIndex == gameData.levelUnlocked && this.isComplete) 
        {
            gameData.levelUnlocked++;
            gameData.fromTrialMode[this.levelIndex - 1] = false;
        }

        if(gameData.fromTrialMode[this.levelIndex - 1])
        {
            gameData.posX = player.transform.localPosition.x;
            gameData.posY = player.transform.localPosition.y;
            gameData.posZ = player.transform.localPosition.z;

            Debug.Log(new Vector3(gameData.posX, gameData.posY, gameData.posZ));
        }
    }
    
    public void Respawn()
    {
        StartCoroutine(DelayControl());
        player.transform.position = intialPosition;
    }

    IEnumerator DelayControl()
    {
        canControl = false;
        yield return new WaitForSeconds(0.5f);
        canControl = true;
    }

    public bool GetCanControl()
    {
        return canControl;
    }

    public void ShowGetRecipeWindow()
    {
        canControl = false;
        LeanTween.value(recipeWindow, UpdateRecipeWindowAlpha, 0.0f, 1.0f, 0.3f).setOnComplete(() =>
        {
            StartCoroutine(ViewRecipe());
        });
    }

    public void GoToNextLevel()
    {
        canControl = false;
        isComplete = true;

        if(levelIndex < 6) 
        {
            LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    private void GoToTrialMode()
    {
        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadSceneAsync(1));
    }

    private void UpdateRecipeWindowAlpha(float alpha) => recipeWindow.GetComponent<CanvasGroup>().alpha = alpha; 

    private void UpdateBlackscreenAlpha(float alpha) => blackScreen.GetComponent<CanvasGroup>().alpha = alpha;
    
    IEnumerator ViewRecipe()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.value(recipeWindow, UpdateRecipeWindowAlpha, 1.0f, 0.0f, 0.3f).setOnComplete(() => 
        {
            canControl = true;
            GoToTrialMode();
        });
    }
}
