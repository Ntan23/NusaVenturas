using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionManager : MonoBehaviour
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

    [Header("For Information Panel")]
    [SerializeField] private TextMeshProUGUI foodOriginText;
    [SerializeField] private TextMeshProUGUI foodNameText;
    [SerializeField] private Image foodImage;
    [SerializeField] private GameObject panel;

    [Header("Buttons")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private Color32 selectedButtonColor;
    [SerializeField] private Button playButton;
    #endregion

    void Start()
    {
        levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);

        isFirstTime = true;

        for(int i = 0; i < buttons.Length; i++)
        {
            if(i < levelUnlocked) buttons[i].interactable = true;
            else if(i >= levelUnlocked) buttons[i].interactable = false;
        }
    }


    public void SelectLevel(int index)
    {
        panel.transform.SetParent(buttons[index].gameObject.transform);

        foodOriginText.text = foods[index].foodOrigin;
        foodNameText.text = foods[index].foodName;
        foodImage.sprite = foods[index].foodSprite;

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
}
