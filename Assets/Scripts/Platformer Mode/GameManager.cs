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
    private bool isPaused;
    private bool canBePressed = true;
    private float posX, posY, posZ;
    [SerializeField] private int levelIndex;
    private int levelUnlocked;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject recipeWindow;
    [SerializeField] private GameObject loseUIWindow;
    [SerializeField] private GameObject winUIWindow;
    [SerializeField] private GameObject errorPopUp;
    [SerializeField] private GameObject pauseMenuUI;
    #endregion

    void Start()
    {
        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() => canControl = true);

        if(fromTrialMode) player.transform.localPosition = new Vector3(this.posX, this.posY, this.posZ);
    
        intialPosition = player.transform.localPosition;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!canBePressed) return; 

            if(!isPaused) Pause();
            if(isPaused) Resume();
        }
    }

    public void LoadData(GameData gameData)
    {
        if(gameData.fromTrialMode[this.levelIndex - 1])
        {
            this.posX = gameData.posX;
            this.posY = gameData.posY;
            this.posZ = gameData.posZ;
        }

        this.fromTrialMode = gameData.fromTrialMode[this.levelIndex - 1];
        this.levelUnlocked = gameData.levelUnlocked;
    }

    public void SaveData(GameData gameData) 
    {
        gameData.levelIndex = this.levelIndex;
        gameData.levelUnlocked = this.levelUnlocked;

        if(this.isComplete) gameData.fromTrialMode[this.levelIndex - 1] = false;
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

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public void OpenShop() => canControl = false;
    public void CloseShop() => canControl = true;
    
    public void ShowGetRecipeWindow()
    {
        canControl = false;
        LeanTween.value(recipeWindow, UpdateRecipeWindowAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() => StartCoroutine(ViewRecipe()));
    }

    public void CheckWinCondition()
    {
        if(levelUnlocked > levelIndex) CompleteGame();
        else if(levelUnlocked <= levelIndex)
        {
            if(!fromTrialMode) LeanTween.moveLocalY(errorPopUp, 430.0f, 0.5f).setOnComplete(() => StartCoroutine(ShowErrorPopUp()));
            else if(fromTrialMode) CompleteGame();
        }
    }

    IEnumerator ShowErrorPopUp()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.moveLocalY(errorPopUp, 770.0f, 0.5f);
    }

    public void GoToNextLevel()
    {
        if(levelIndex < 6) LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private void GoToTrialMode() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("TrialCookingMode"));

    public void ReloadLevel() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("Level" + levelIndex.ToString()));

    public void GoToMainMenu() 
    {
        Time.timeScale = 1.0f;

        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("Main Menu"));
    }

    private void Pause()
    {
        canBePressed = false;
        canControl = false;

        LeanTween.value(pauseMenuUI, UpdatePauseMenuUIAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
        {
            pauseMenuUI.GetComponent<CanvasGroup>().interactable = true;
            pauseMenuUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
            isPaused = true;
            canBePressed = true;

            Time.timeScale = 0.0f;
        });
    }

    public void Resume()
    {
        canBePressed = false;
        Time.timeScale = 1.0f;

        LeanTween.value(pauseMenuUI, UpdatePauseMenuUIAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
        {
            pauseMenuUI.GetComponent<CanvasGroup>().interactable = false;
            pauseMenuUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
            canControl = true;
            isPaused = false;
            canBePressed = true;
        });
    }

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

        if(levelUnlocked == levelIndex) levelUnlocked++;

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
    
    private void UpdatePauseMenuUIAlpha(float alpha) => pauseMenuUI.GetComponent<CanvasGroup>().alpha = alpha;

    IEnumerator ViewRecipe()
    {
        yield return new WaitForSeconds(0.8f);
        LeanTween.value(recipeWindow, UpdateRecipeWindowAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() => 
        {
            GoToTrialMode();
        });
    }
}
