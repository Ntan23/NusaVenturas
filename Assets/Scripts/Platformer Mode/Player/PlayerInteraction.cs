using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables
    [Header("For Detection")]
    [SerializeField] private Transform detectionPoint;
    private const float detectionRadius = 0.25f;
    [SerializeField] private LayerMask detectionLayer;
    private GameObject detectedObject;

    [Header("For Examine")]
    [SerializeField] private GameObject examineWindow;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI examineDescription;
    private bool isExamining;

    [Header("Others")]
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject examineText;
    [SerializeField] private GameObject pickUpText;

    private GameManager gm;
    #endregion

    void Start()
    {
        gm = GameManager.instance;
        
        examineWindow.SetActive(false);
        examineText.SetActive(false);
        interactText.SetActive(false);
    }

    void Update()
    { 
        if(!gm.GetCanControl()) return;

        if(DetectObjects())
        {
            if(Input.GetKeyDown(KeyCode.E)) detectedObject.GetComponent<Item>().InteractObject();
            
            return;
        }
        else if(!DetectObjects())
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(isExamining) DisableExamineWindow();   
            }
        }
    }

    private bool DetectObjects()
    {
        Collider2D detected = Physics2D.OverlapCircle(detectionPoint.position,detectionRadius,detectionLayer);
        
        if(detected == null)
        {
            detectedObject = null;
            examineText.SetActive(false);
            interactText.SetActive(false);
            pickUpText.SetActive(false);
            return false;
        }
        
        if(detected != null)
        {
            detectedObject = detected.gameObject;

            ShowText();
        } 
        return true;
    }

    private void ShowText()
    {
        switch(detectedObject.tag)
        {
            case "Examine" :
                examineText.SetActive(true);
                interactText.SetActive(false);
                pickUpText.SetActive(false);

                if(isExamining) examineText.SetActive(false);
                
                break;
            case "PickUp" :
                examineText.SetActive(false);
                interactText.SetActive(false);
                pickUpText.SetActive(true);

                break;
            default :
                interactText.SetActive(true);
                examineText.SetActive(false);
                pickUpText.SetActive(false);

                if(isExamining)
                {
                    interactText.SetActive(false);
                    examineText.SetActive(false);
                    pickUpText.SetActive(false);
                }

                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.black;
        Gizmos.DrawSphere(detectionPoint.position,detectionRadius);
    }

    public void ExamineItem(Item item)
    {
        if(!isExamining)
        {
            itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            examineDescription.text = item.descriptionText;

            examineWindow.SetActive(true);
            isExamining = true;
        }
        else if(isExamining)
        {
            examineWindow.SetActive(false);
            isExamining = false;
        }
    }

    private void DisableExamineWindow()
    {
        examineWindow.SetActive(false);
        isExamining = false;
    }

    public bool GetIsExamining()
    {
        return isExamining;
    }
}
