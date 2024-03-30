using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    #region Variables
    private GameObject player;
    private Vector3 intialPosition;
    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        intialPosition = player.transform.position;
    }

    void Update()
    {
        
    }

    public void Respawn()
    {
        player.transform.position = intialPosition;
    }
}
