using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] private UnityEvent activateEvent;
    [SerializeField] private UnityEvent deactivateEvent;
    private bool isOpen;
    private Animator leverAnimator;

    void Start() => leverAnimator = GetComponent<Animator>();

    public void InteractWithLever()
    {
        if(!isOpen)
        {
            leverAnimator.Play("LeverActivate");
            activateEvent?.Invoke();
        }

        if(isOpen)
        {
            leverAnimator.Play("LeverDeactivate");
            deactivateEvent?.Invoke();
        }

        isOpen = !isOpen;
    }
}
