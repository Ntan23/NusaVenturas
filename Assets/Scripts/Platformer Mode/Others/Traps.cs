using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.GetComponent<PlayerHealth>().GetIsInvulnerable()) 
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.left * 500.0f;
            other.GetComponent<PlayerHealth>().LoseLive();
        }
    }
}
