using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour, IData
{
    #region Singleton
    public static MainMenuManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    #region Variables
    private int collectedRecipeCount;
    private bool canControl;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject errorPopUp;
    [SerializeField] private TextMeshProUGUI errorText;
    #endregion
    
    void Start() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() => canControl = true);
    
    public void LoadData(GameData gameData) => this.collectedRecipeCount = gameData.collectedRecipeCount;
    
    public void SaveData(GameData gameData)
    {
    }

    public void OpenShopOrOtherWindow() => canControl = false;
    public void CloseShopOrOtherWindow() => canControl = true;

    public void GoToLevelSelection() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));

    public void QuitGame() => LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => Application.Quit());

    public void GoToEndlessMode()
    {
        if(collectedRecipeCount < 6) 
        {
            errorText.text = "Need " +  (6 - collectedRecipeCount).ToString() + " recipe(s) more";

            LeanTween.moveLocalY(errorPopUp, 430.0f, 0.5f).setOnComplete(() => StartCoroutine(ShowErrorPopUp()));
        }
        if(collectedRecipeCount == 6) LeanTween.value(blackScreen, UpdateBlackscreenAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => SceneManager.LoadScene("EndlessCookingMode2"));
    }

    IEnumerator ShowErrorPopUp()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.moveLocalY(errorPopUp, 770.0f, 0.5f);
    }

    private void UpdateBlackscreenAlpha(float alpha) => blackScreen.GetComponent<CanvasGroup>().alpha = alpha;

    public bool GetCanControl() 
    {
        return canControl;
    }
}
