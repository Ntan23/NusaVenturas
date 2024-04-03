using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player")) collisionInfo.gameObject.GetComponent<PlayerHealth>().LoseLive();
    }
}
