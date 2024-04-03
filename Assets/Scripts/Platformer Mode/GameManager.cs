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
    [SerializeField] private int levelIndex;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject recipeWindow;
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        intialPosition = player.transform.position;

        LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() => canControl = true);
    }

    public void LoadData(GameData gameData)
    {
        Debug.Log("Load");
    }

    public void SaveData(GameData gameData) 
    {
        if(levelIndex == gameData.levelUnlocked && isComplete) gameData.levelUnlocked++;
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

    private void UpdateRecipeWindowAlpha(float alpha) => recipeWindow.GetComponent<CanvasGroup>().alpha = alpha; 

    private void UpdateBlackscreenAlpha(float alpha) => blackScreen.GetComponent<CanvasGroup>().alpha = alpha;
    
    IEnumerator ViewRecipe()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.value(recipeWindow, UpdateRecipeWindowAlpha, 1.0f, 0.0f, 0.3f).setOnComplete(() => 
        {
            canControl = true;
        });
    }
}
