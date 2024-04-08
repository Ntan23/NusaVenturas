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

        numberOfFlashes = 3.0f * iFramesDuration;

        maxHealth = healthCount;

        UpdateHealthUI();

        Physics2D.IgnoreLayerCollision(0,8,false);
    }

    public void LoadData(GameData gameData)
    {
        this.healthCount = gameData.healthCount;
        this.iFramesDuration = gameData.immunityTime;
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
        if(healthCount >= 0)
        {
            healthBar.fillAmount = healthCount/maxHealth;

            healthText.text = healthCount.ToString() + " / " + maxHealth.ToString();
        }
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
}
