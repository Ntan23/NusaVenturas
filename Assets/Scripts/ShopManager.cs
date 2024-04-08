using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour, IData
{
    #region Variables
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject shopWindow;
    private int maxLevel = 3;
    private float cookingSpeed;
    private int cookingSpeedLevel;
    private float cookingSpeedUpgradeCost;
    private float immunityTime;
    private int immunityLevel;
    private float immunityTimeUpgradeCost;
    private float jumpPower;
    private int jumpLevel;
    private float jumpPowerUpgradeCost;
    private float healthCount;
    private int healthLevel;
    private float healthUpgradeCost;
    private float coinCount;
    private GameManager gm;
    #endregion

    void Start() => gm = GameManager.instance;

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
        gm.OpenShop();
        LeanTween.value(shopWindow, UpdateShopWindowAlpha, 0.0f, 1.0f, 0.6f).setOnComplete(() => 
        {
            shopWindow.GetComponent<CanvasGroup>().interactable = true;
        });
    }

    public void CloseShopWindow() 
    {
        LeanTween.value(shopWindow, UpdateShopWindowAlpha, 0.0f, 1.0f, 0.6f).setOnComplete(() => {
            gm.CloseShop();
            shopWindow.GetComponent<CanvasGroup>().interactable = false;
        });
    }
    
    private void UpdateShopWindowAlpha(float alpha) => shopWindow.GetComponent<CanvasGroup>().alpha = alpha;

    public void UpgradeHealth()
    {
        if(healthLevel < maxLevel && coinCount >= healthUpgradeCost)
        {
            healthCount++;
            healthLevel++;
            if(playerHealth != null) playerHealth.UpgradeHealth(healthCount);

            healthUpgradeCost *= 1.5f;
        }
    }

    public void UpgradeJumpPower()
    {
        if(jumpLevel < maxLevel && coinCount >= jumpPowerUpgradeCost)
        {
            jumpPower += 0.5f;
            jumpLevel++;
            if(playerMovement != null) playerMovement.UpdateJumpPower(jumpPower);

            jumpPowerUpgradeCost *= 1.5f;
        }
    }

    public void UpgradeImmunityTime()
    {
        if(immunityLevel < maxLevel && coinCount >= immunityTimeUpgradeCost)
        {
            immunityTime += 0.5f;
            immunityLevel++;
            if(playerHealth != null) playerHealth.UpgradeImmunityTime(immunityTime);

            immunityTimeUpgradeCost *= 1.5f;
        }
    }

    public void UpgradeCookingSpeed()
    {
        if(cookingSpeedLevel < maxLevel && coinCount >= cookingSpeedUpgradeCost) 
        {
            cookingSpeed -= 0.5f;

            cookingSpeedUpgradeCost *= 1.5f;
        }
    }
}
