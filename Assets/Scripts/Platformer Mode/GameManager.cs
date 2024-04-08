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
    private Vector3 intialPosition;
    private bool canControl;
    private bool isComplete;
    private bool fromTrialMode;
    private float posX, posY, posZ;
    [SerializeField] private int levelIndex;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject recipeWindow;
    [SerializeField] private GameObject loseUIWindow;
    [SerializeField] private GameObject winUIWindow;
    #endregion

    void Start()
    {
        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() => canControl = true);

        if(fromTrialMode) player.transform.localPosition = new Vector3(this.posX, this.posY, this.posZ);
        
        intialPosition = player.transform.localPosition;
    }

    public void LoadData(GameData gameData)
    {
        Debug.Log(gameData.fromTrialMode[this.levelIndex - 1]);

        if(gameData.fromTrialMode[this.levelIndex - 1])
        {
            this.posX = gameData.posX;
            this.posY = gameData.posY;
            this.posZ = gameData.posZ;
        }

        this.fromTrialMode = gameData.fromTrialMode[this.levelIndex - 1];
    }

    public void SaveData(GameData gameData) 
    {
        gameData.levelIndex = this.levelIndex;

        if(this.levelIndex == gameData.levelUnlocked && this.isComplete) 
        {
            gameData.levelUnlocked++;
            gameData.fromTrialMode[this.levelIndex - 1] = false;
        }
    }
    
    public void Respawn()
    {
        StartCoroutine(DelayControl());
        player.transform.position = intialPosition;
        player.GetComponent<PlayerHealth>().LoseLive();
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

    public bool GetFromTrialMode()
    {
        return fromTrialMode;
    }

    public void OpenShop() => canControl = false;
    public void CloseShop() => canControl = true;
    
    public void ShowGetRecipeWindow()
    {
        canControl = false;
        LeanTween.value(recipeWindow, UpdateRecipeWindowAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() => StartCoroutine(ViewRecipe()));
    }

    public void GoToNextLevel()
    {
        if(levelIndex < 6) LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private void GoToTrialMode() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("TrialCookingMode"));

    public void ReloadLevel() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("Level" + levelIndex.ToString()));

    public void GoToMainMenu() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("Main Menu"));
    
    public void GameOver()
    {
        canControl = false;

        LeanTween.value(loseUIWindow, UpdateLoseUIWindowAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => 
        {
            loseUIWindow.GetComponent<CanvasGroup>().interactable = true;
            loseUIWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
        });
    }

    public void CompleteGame()
    {
        canControl = false;
        isComplete = true;

        LeanTween.value(winUIWindow, UpdateWinUIWindowAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => 
        {
            winUIWindow.GetComponent<CanvasGroup>().interactable = true;
            winUIWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
        });
    }

    private void UpdateRecipeWindowAlpha(float alpha) => recipeWindow.GetComponent<CanvasGroup>().alpha = alpha; 

    private void UpdateBlackscreenAlpha(float alpha) => blackScreen.GetComponent<CanvasGroup>().alpha = alpha;

    private void UpdateLoseUIWindowAlpha(float alpha) => loseUIWindow.GetComponent<CanvasGroup>().alpha = alpha;

    private void UpdateWinUIWindowAlpha(float alpha) => winUIWindow.GetComponent<CanvasGroup>().alpha = alpha;
    
    IEnumerator ViewRecipe()
    {
        yield return new WaitForSeconds(0.8f);
        LeanTween.value(recipeWindow, UpdateRecipeWindowAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() => 
        {
            GoToTrialMode();
        });
    }
}
