using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditText;
    [SerializeField] private float textTimeToReachTheEnd;
    [SerializeField] private float targetYLocation;
    private MainMenuManager mm;

    void Start() => mm = MainMenuManager.instance;

    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;
    
    public void OpenCredits()
    {
        mm.OpenShopOrOtherWindow();
        GetComponent<CanvasGroup>().blocksRaycasts = true;
       
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
        {
            LeanTween.moveLocalY(creditText, targetYLocation, textTimeToReachTheEnd).setLoopClamp();
            GetComponent<CanvasGroup>().interactable = true;
        });
    }

    public void CloseCredits() 
    {
        GetComponent<CanvasGroup>().interactable = false;
        LeanTween.cancel(creditText); 

        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            mm.CloseShopOrOtherWindow();
        });
        creditText.transform.localPosition = new Vector3(0.0f, -targetYLocation, 0.0f);
    }
}

