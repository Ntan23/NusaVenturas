using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour, IData
{
    private float healthCount;
    private float maxHealth;
    private bool isInvunerable;

    [Header("For IFrames")]
    [SerializeField] private float iFramesDuration;
    private float numberOfFlashes;
    [SerializeField] private SpriteRenderer[] playerSprite;

    [Header("For UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    private GameManager gm;

    void Start() 
    {
        gm = GameManager.instance;

        numberOfFlashes = iFramesDuration * 3.0f;

        UpdateHealthUI();

        Physics2D.IgnoreLayerCollision(0,8,false);
    }

    public void LoadData(GameData gameData)
    {
        if(!gameData.fromTrialMode[gameData.levelIndex - 1]) this.healthCount = gameData.healthCount;
        if(gameData.fromTrialMode[gameData.levelIndex - 1]) this.healthCount = gameData.savedHealth;
        this.iFramesDuration = gameData.immunityTime;

        this.maxHealth = gameData.healthCount;
    }

    public void SaveData(GameData gameData)
    {
        
    }

    public void LoseLive() 
    {
        healthCount--;
        UpdateHealthUI();

        if(healthCount <= 0) 
        {
            Physics2D.IgnoreLayerCollision(0,8,true);
            gm.GameOver();
            return;
        }
        if(healthCount > 0) StartCoroutine(Invunerability());
    }

    private void UpdateHealthUI()
    {
        if(healthCount >= 0 && healthBar != null)
        {
            healthBar.fillAmount = healthCount/maxHealth;

            healthText.text = healthCount.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void UpgradeHealth(float value)
    {
        maxHealth = value;
        healthCount = value;

        UpdateHealthUI();
    }

    public void UpgradeImmunityTime(float value)
    {
        iFramesDuration = value;
        numberOfFlashes = iFramesDuration * 3;
    }

    IEnumerator Invunerability()
    {
        isInvunerable = true;

        Physics2D.IgnoreLayerCollision(0,8,true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            foreach(SpriteRenderer spriteRenderer in playerSprite) spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/(numberOfFlashes*2));
            foreach(SpriteRenderer spriteRenderer in playerSprite) spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/(numberOfFlashes*2));
        }
        Physics2D.IgnoreLayerCollision(0,8,false);

        isInvunerable = false;
    }

    public bool GetIsInvulnerable()
    {
        return isInvunerable;
    }

    public float GetHealthCount()
    {
        return healthCount;
    }
}
