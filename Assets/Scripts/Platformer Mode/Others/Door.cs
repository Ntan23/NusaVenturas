using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameManager gm;

    void Start() => gm = GameManager.instance;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) gm.GoToNextLevel();
    }
}
