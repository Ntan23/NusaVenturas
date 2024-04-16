using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour, IData
{
    [SerializeField] private AudioMixer MainMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle fullscreenToogle;
    private int fullscreenIndicator;
    private float masterVolume;
    private float bgmVolume;
    private float sfxVolume;
    private int screenWidth;
    private int screenHeight;
    private bool isOpen;
    [SerializeField] private bool isInTheMainMenu;
    private MainMenuManager mm;

    private void Start()
    {
        mm = MainMenuManager.instance;

        screenWidth = Screen.currentResolution.width;
        screenHeight = Screen.currentResolution.height;

        if(fullscreenIndicator == 1) 
        {
            fullscreenToogle.isOn = true;
            Screen.fullScreen = true;
            Screen.SetResolution(screenWidth, screenHeight, true);
        }
        
        if(fullscreenIndicator == 0) 
        {
            fullscreenToogle.isOn = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(Mathf.RoundToInt(screenWidth / 1.5f), Mathf.RoundToInt(screenHeight / 1.5f), false);
        }
        
        MainMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        MainMixer.SetFloat("BGMVolume", Mathf.Log10(bgmVolume) * 20);
        MainMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);

        masterSlider.value = masterVolume;
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;
    }

    public void LoadData(GameData gameData)
    {
        this.fullscreenIndicator = gameData.fullscreenIndicator;
        this.masterVolume = gameData.masterVolume;
        this.bgmVolume = gameData.bgmVolume;
        this.sfxVolume = gameData.sfxVolume;
    }

    public void SaveData(GameData gameData)
    {
        gameData.fullscreenIndicator = this.fullscreenIndicator;
        gameData.masterVolume = this.masterVolume;
        gameData.bgmVolume = this.bgmVolume;
        gameData.sfxVolume = this.sfxVolume;
    }
    
    public void UpdateMasterSound(float value)
    {
        MainMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        masterVolume = value;
    }

    public void UpdateBGMSound(float value)
    {
        MainMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        bgmVolume = value;
    }

    public void UpdateSFXSound(float value)
    {
        MainMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        sfxVolume = value;
    }

    public void FullScreenControl(bool isFullscreen)
    {
        if(isFullscreen) 
        {
            Screen.fullScreen = isFullscreen;
            Screen.SetResolution(screenWidth, screenHeight, true);
            fullscreenIndicator = 1;
        }
        else if (!isFullscreen) 
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(Mathf.RoundToInt(screenWidth / 1.5f), Mathf.RoundToInt(screenHeight / 1.5f), false);
            fullscreenIndicator = 0;
        }
    }

    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenSettings()
    {
        if(Time.timeScale == 0) Time.timeScale = 1.0f;
        if(mm != null) mm.OpenShopOrOtherWindow();
        isOpen = true;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() => 
        {
            GetComponent<CanvasGroup>().interactable = true;
            if(!isInTheMainMenu) Time.timeScale = 0.0f;
        });
    }
    
    public void CloseSettings() 
    {
        if(Time.timeScale == 0) Time.timeScale = 1.0f;

        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() => 
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<CanvasGroup>().interactable = false;
            isOpen = false;
            
            if(mm != null) mm.CloseShopOrOtherWindow();
            if(!isInTheMainMenu) Time.timeScale = 0.0f;
        });
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
}
