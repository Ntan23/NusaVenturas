using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour, IData
{
    #region Singleton
    public static LevelSelectionManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    #region Variables
    [SerializeField] private FoodSO[] foods;
    private int levelUnlocked;
    private int tempIndex;
    private bool isFirstTime;
    private bool collected;
    private bool[] recipeCollected;
    private bool[] isInTrialMode;
    private bool[] fromTrialMode;

    [Header("For Information Panel")]
    [SerializeField] private TextMeshProUGUI foodOriginText;
    [SerializeField] private TextMeshProUGUI foodNameText;
    [SerializeField] private Image foodImage;
    [SerializeField] private GameObject panel;

    [Header("Buttons")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private Color32 selectedButtonColor;
    [SerializeField] private Button playButton;
    private TextMeshProUGUI playButtonText;
    #endregion

    void Start()
    {
        playButtonText = playButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        isFirstTime = true;

        for(int i = 0; i < buttons.Length; i++)
        {
            if(i < levelUnlocked) buttons[i].interactable = true;
            else if(i >= levelUnlocked) buttons[i].interactable = false;
        }
    }

    public void LoadData(GameData gameData)
    {
        this.levelUnlocked = gameData.levelUnlocked;

        this.isInTrialMode = new bool[gameData.isInTrialMode.Length];
        this.fromTrialMode = new bool[gameData.fromTrialMode.Length];

        for(int i = 0; i < gameData.isInTrialMode.Length; i++) this.isInTrialMode[i] = gameData.isInTrialMode[i];

        for(int i = 0; i < gameData.fromTrialMode.Length; i++) this.fromTrialMode[i] = gameData.fromTrialMode[i];

        recipeCollected = new bool[foods.Length];
        
        for(int i = 0; i < foods.Length; i++)
        {
            gameData.recipeCollected.TryGetValue(foods[i].foodRecipeID,out collected);
            
            if(collected) recipeCollected[i] = true;
            if(!collected) recipeCollected[i] = false;
        }
    }

    public void SaveData(GameData gameData)
    {
        Debug.Log("Save");
    }

    public void SelectLevel(int index)
    {
        panel.transform.SetParent(buttons[index].gameObject.transform);

        foodOriginText.text = foods[index].foodOrigin;
        foodImage.sprite = foods[index].foodSpriteWithFrame;

        if(recipeCollected[index]) 
        {
            foodNameText.text = foods[index].foodName;
            foodImage.color = Color.white;
            
            if(isInTrialMode[index]) playButtonText.text = "Continue";
            else if(fromTrialMode[index])  playButtonText.text = "Continue";
            else if(levelUnlocked > index) playButtonText.text = "Re-Adventure";
        }
        else if(!recipeCollected[index])
        {
            foodNameText.text = "?????";
            foodImage.color = Color.black;
            playButtonText.text = "Find Recipe";
        }

        if(isFirstTime)
        {
            buttons[index].GetComponent<Image>().color = selectedButtonColor;
            
            LeanTween.scale(buttons[index].gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.3f).setEaseSpring();
            
            LeanTween.scaleX(panel, 1.0f, 0.5f).setEaseInOutExpo().setOnComplete(() => {
                playButton.interactable = true;

                tempIndex = index;
                isFirstTime = false;
            });
        }

        if(!isFirstTime)
        {
            panel.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
            buttons[tempIndex].GetComponent<Image>().color = Color.red;

            LeanTween.scale(buttons[tempIndex].gameObject, Vector3.one, 0.3f).setEaseSpring();
            
            buttons[index].GetComponent<Image>().color = selectedButtonColor;

            LeanTween.scale(buttons[index].gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.3f).setEaseSpring();

            LeanTween.scaleX(panel, 1.0f, 0.5f).setEaseInOutExpo().setOnComplete(() => {
                playButton.interactable = true;
                
                tempIndex = index;
            });
        }
    }

    public void GoToLevel() 
    {
        if(isInTrialMode[tempIndex]) SceneManager.LoadScene(1);
        else SceneManager.LoadScene("Level" + (tempIndex + 1).ToString());
    }
}
