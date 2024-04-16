using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour, IData
{
    #region Variables
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private bool isInMainMenu;
    private bool isOpen;
    #region Stats&UpgradeVariables
    private int maxLevel = 3;
    private float cookingSpeed;
    private int cookingSpeedLevel;
    private float nextCookingSpeed;
    private float cookingSpeedUpgradeCost;
    private float immunityTime;
    private int immunityLevel;
    private float nextImmunityTime;
    private float immunityTimeUpgradeCost;
    private float jumpPower;
    private float nextJumpPower;
    private int jumpLevel;
    private float jumpPowerUpgradeCost;
    private float healthCount;
    private int healthLevel;
    private float nextHealth;
    private float healthUpgradeCost;
    private float coinCount;
    #endregion
    [Header("UIs")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI[] upgradeInfoText;
    [SerializeField] private TextMeshProUGUI[] upgradeCostText;
    [SerializeField] private Button[] upgradeButton;
    [SerializeField] private GameObject[] maxButton;
    private GameManager gm;
    private MainMenuManager mm;
    private AudioManager am;
    #endregion

    void Start() 
    {
        if(!isInMainMenu) gm = GameManager.instance;
        if(isInMainMenu) mm = MainMenuManager.instance;

        am = AudioManager.instance;

        nextCookingSpeed = cookingSpeed - 0.5f;
        nextImmunityTime = immunityTime + 0.5f;
        nextJumpPower = jumpPower + 0.5f;
        nextHealth = healthCount + 1.0f;

        UpdateCoinTextUI();
        UpdateShopUI();
    }

    public void LoadData(GameData gameData)
    {
        this.cookingSpeed = gameData.cookingSpeed;
        this.cookingSpeedLevel = gameData.cookingSpeedLevel;
        this.cookingSpeedUpgradeCost = gameData.cookingSpeedUpgradeCost;
        this.immunityTime = gameData.immunityTime;
        this.immunityLevel = gameData.immunityLevel;
        this.immunityTimeUpgradeCost = gameData.immunityTimeUpgradeCost;
        this.jumpPower = gameData.jumpPower;
        this.jumpLevel = gameData.jumpLevel;
        this.jumpPowerUpgradeCost = gameData.jumpPowerUpgradeCost;
        this.healthCount = gameData.healthCount;
        this.healthLevel = gameData.healthLevel;
        this.healthUpgradeCost = gameData.healthUpgradeCost;
        this.coinCount = gameData.coinCount;
    }

    public void SaveData(GameData gameData)
    {
        gameData.cookingSpeed = this.cookingSpeed;
        gameData.cookingSpeedLevel = this.cookingSpeedLevel;
        gameData.cookingSpeedUpgradeCost = this.cookingSpeedUpgradeCost;
        gameData.immunityTime = this.immunityTime;
        gameData.immunityLevel = this.immunityLevel;
        gameData.immunityTimeUpgradeCost = this.immunityTimeUpgradeCost;
        gameData.jumpPower = this.jumpPower;
        gameData.jumpLevel = this.jumpLevel;
        gameData.jumpPowerUpgradeCost = this.jumpPowerUpgradeCost;
        gameData.healthCount = this.healthCount;
        gameData.healthLevel = this.healthLevel;
        gameData.healthUpgradeCost = this.healthUpgradeCost;
        gameData.coinCount = this.coinCount;
    }

    public void OpenShopWindow() 
    {
        if(!isOpen)
        {
            isOpen = true;
            
            if(!isInMainMenu) gm.OpenShop();
            if(isInMainMenu) mm.OpenShop();

            LeanTween.value(shopWindow, UpdateShopWindowAlpha, 0.0f, 1.0f, 0.6f).setOnComplete(() => 
            {
                shopWindow.GetComponent<CanvasGroup>().interactable = true;
                shopWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
            });
        }
    }

    public void CloseShopWindow() 
    {
        LeanTween.value(shopWindow, UpdateShopWindowAlpha, 1.0f, 0.0f, 0.6f).setOnComplete(() => {
            if(!isInMainMenu) gm.CloseShop();
            if(isInMainMenu) mm.CloseShop();
            
            shopWindow.GetComponent<CanvasGroup>().interactable = false;
            shopWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;

            isOpen = false;
        });
    }
    
    private void UpdateShopWindowAlpha(float alpha) => shopWindow.GetComponent<CanvasGroup>().alpha = alpha;

    public void UpgradeHealth()
    {
        if(healthLevel < maxLevel && coinCount >= healthUpgradeCost)
        {
            am.Play("Upgrade");

            coinCount -= healthUpgradeCost;
            UpdateCoinTextUI();

            healthCount = nextHealth;
            healthLevel++;

            nextHealth++;

            if(playerHealth != null) playerHealth.UpgradeHealth(healthCount);

            healthUpgradeCost *= 1.5f;
        }

        UpdateShopUI();
    }

    public void UpgradeJumpPower()
    {
        if(jumpLevel < maxLevel && coinCount >= jumpPowerUpgradeCost)
        {
            am.Play("Upgrade");

            coinCount -= jumpPowerUpgradeCost;
            UpdateCoinTextUI();

            jumpPower = nextJumpPower;
            jumpLevel++;

            nextJumpPower += 0.5f;

            if(playerMovement != null) playerMovement.UpdateJumpPower(jumpPower);

            jumpPowerUpgradeCost *= 1.5f;
        }

        coinCount = 100;

        UpdateShopUI();
    }

    public void UpgradeImmunityTime()
    {
        if(immunityLevel < maxLevel && coinCount >= immunityTimeUpgradeCost)
        {
            am.Play("Upgrade");

            coinCount -= immunityTimeUpgradeCost;
            UpdateCoinTextUI();

            immunityTime = nextImmunityTime;
            immunityLevel++;

            nextImmunityTime += 0.5f;

            if(playerHealth != null) playerHealth.UpgradeImmunityTime(immunityTime);

            immunityTimeUpgradeCost *= 1.5f;
        }

        UpdateShopUI();
    }

    public void UpgradeCookingSpeed()
    {
        if(cookingSpeedLevel < maxLevel && coinCount >= cookingSpeedUpgradeCost) 
        {
            am.Play("Upgrade");

            coinCount -= cookingSpeedUpgradeCost;
            UpdateCoinTextUI();

            cookingSpeed = nextCookingSpeed;
            cookingSpeedLevel++;

            nextCookingSpeed -= 0.5f;

            cookingSpeedUpgradeCost *= 1.5f;
        }

        UpdateShopUI();
    }

    private void UpdateCoinTextUI() => coinsText.text = coinCount.ToString("0.00");

    private void UpdateShopUI()
    {
        if(coinCount < healthUpgradeCost) upgradeButton[0].interactable = false;
        if(coinCount < immunityTimeUpgradeCost) upgradeButton[1].interactable = false;
        if(coinCount < jumpPowerUpgradeCost) upgradeButton[2].interactable = false;
        if(coinCount < cookingSpeedUpgradeCost) upgradeButton[3].interactable = false;

        #region Health
        if(healthLevel < maxLevel) 
        {   
            upgradeInfoText[0].text = $"<color=red><u>Max Health </u><size=16>[ Lv {healthLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {healthCount.ToString()}\n</color><color=orange>Upgraded : {nextHealth.ToString()}</color>";
            
            upgradeCostText[0].text = healthUpgradeCost.ToString("0.00");
        }

        if(healthLevel == maxLevel)
        {
            upgradeInfoText[0].text = $"<color=red><u>Max Health </u><size=16>[ Lv {healthLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {healthCount.ToString()}</color>";

            upgradeButton[0].gameObject.SetActive(false);
            maxButton[0].SetActive(true);
        }
        #endregion

        #region ImmunityTime
        if(immunityLevel < maxLevel) 
        {   
            upgradeInfoText[1].text = $"<color=red><u>Immunity Time </u><size=16>[ Lv {immunityLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {immunityTime.ToString()} s\n</color><color=orange>Upgraded : {nextImmunityTime.ToString()} s</color>";
            
            upgradeCostText[1].text = immunityTimeUpgradeCost.ToString("0.00");
        }

        if(immunityLevel == maxLevel)
        {
            upgradeInfoText[1].text = $"<color=red><u>Immunity Time </u><size=16>[ Lv {immunityLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {immunityTime.ToString()} s</color>";

            upgradeButton[1].gameObject.SetActive(false);
            maxButton[1].SetActive(true);
        }
        #endregion

        #region JumpPower
        if(jumpLevel < maxLevel) 
        {   
            upgradeInfoText[2].text = $"<color=red><u>Jump Power </u><size=16>[ Lv {jumpLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {jumpPower.ToString()}\n</color><color=orange>Upgraded : {nextJumpPower.ToString()}</color>";
            
            upgradeCostText[2].text = healthUpgradeCost.ToString("0.00");
        }

        if(jumpLevel == maxLevel)
        {
            upgradeInfoText[2].text = $"<color=red><u>Jump Power </u><size=16>[ Lv {jumpLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {jumpPower.ToString()}";

            upgradeButton[2].gameObject.SetActive(false);
            maxButton[2].SetActive(true);
        }
        #endregion
    
        #region CookingSpeed
        if(cookingSpeedLevel < maxLevel) 
        {   
            upgradeInfoText[3].text = $"<color=red><u>Cooking Time </u><size=16>[ Lv {cookingSpeedLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {cookingSpeed.ToString()} s\n</color><color=orange>Upgraded : {nextCookingSpeed.ToString()} s</color>";
            
            upgradeCostText[3].text = healthUpgradeCost.ToString("0.00");
        }

        if(cookingSpeedLevel == maxLevel)
        {
            upgradeInfoText[3].text = $"<color=red><u>Cooking Time </u><size=16>[ Lv {cookingSpeedLevel.ToString()} / {maxLevel.ToString()} ]\n</size></color><color=green>Current : {cookingSpeed.ToString()} s</color>";

            upgradeButton[3].gameObject.SetActive(false);
            maxButton[3].SetActive(true);
        }
        #endregion
    }
}
