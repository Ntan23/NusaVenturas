using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player") && !collisionInfo.gameObject.GetComponent<PlayerHealth>().GetIsInvulnerable()) 
        {
            playerMovement.SetKnockback();

            if(collisionInfo.gameObject.transform.position.x <= transform.position.x) playerMovement.SetKnockFromLeftValue(false);
            if(collisionInfo.gameObject.transform.position.x > transform.position.x) playerMovement.SetKnockFromLeftValue(true);

            playerHealth.LoseLive();
        }
    }
}
