using UnityEngine;

public class Pit : MonoBehaviour
{
    private GameManager gm;

    void Start() => gm = GameManager.instance;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) gm.Respawn();
    }
}
