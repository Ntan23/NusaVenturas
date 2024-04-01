using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    #region Variables
    private enum InteractionType{
        Lever, Examine, PickUp
    }

    [Header("Attributes")]
    [SerializeField] private InteractionType type;
    
    [Header("For Examine")]
    public string descriptionText;
    
    [Header("Custom Events")]
    public UnityEvent customEvent;
    private PlayerInteraction playerInteraction;
    private Lever leverScript;
    #endregion

    void Start()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();

        switch(type)
        {
            case InteractionType.Examine :
                this.tag = "Examine";
                break; 
            case InteractionType.Lever :
                this.tag = "Lever";
                break; 
            case InteractionType.PickUp :
                this.tag = "PickUp";
                break;
            default :
                break;
        }
    }

    void OnEnable() => leverScript = GetComponent<Lever>();

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    public void InteractObject()
    {
        switch(type)
        {
            case InteractionType.Lever :
                leverScript.InteractWithLever();
                break;
            case InteractionType.Examine :
                //Call The Examine Item Function In The Interaction System
                playerInteraction.ExamineItem(this);
                break; 
            default :
                break;
        }

        //Invoke(Call) Custom Events
        if(!playerInteraction.GetIsExamining())
        {
            customEvent?.Invoke();
        }
    }
}