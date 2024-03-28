using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();   
    } 
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && playerMovement.GetIsAirborne()) 
        {
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            other.gameObject.SetActive(false);
        }
    }
}
