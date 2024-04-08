using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameManager gm;
    private bool fromTrialMode;

    void Start() => gm = GameManager.instance;
    
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player")) gm.GoToNextLevel();
    // }

    public void CheckCondition()
    {
        if(!gm.GetFromTrialMode()) Debug.Log("You Need To Find A Recipe");
        else if(gm.GetFromTrialMode()) gm.CompleteGame();
    }

}
