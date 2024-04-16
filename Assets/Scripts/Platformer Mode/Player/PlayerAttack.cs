using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D rb;
    private AudioManager am;

    void Start() => am = AudioManager.instance;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && playerMovement.GetIsAirborne()) 
        {
            am.Play("Attack");
            
            rb.velocity = Vector2.up * 6.0f;
            other.gameObject.SetActive(false);
        }
    }
}
