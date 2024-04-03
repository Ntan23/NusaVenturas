using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int healthCount;
    private bool isInvunerable;

    [Header("For IFrames")]
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private SpriteRenderer[] playerSprite;


    public void LoseLive() => StartCoroutine(Invunerability());

    IEnumerator Invunerability()
    {
        isInvunerable = true;

        Physics2D.IgnoreLayerCollision(0,8,true);

        for (int i=0;i<numberOfFlashes;i++)
        {
            foreach(SpriteRenderer spriteRenderer in playerSprite) spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/(numberOfFlashes*2));
            foreach(SpriteRenderer spriteRenderer in playerSprite) spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/(numberOfFlashes*2));
        }
        Physics2D.IgnoreLayerCollision(0,8,false);

        isInvunerable=false;
    }
}
