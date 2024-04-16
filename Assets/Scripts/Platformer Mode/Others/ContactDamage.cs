using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;

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
