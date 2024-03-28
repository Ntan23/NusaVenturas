using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> points;
    private int NextLocation = 0;
    private int LocationChangerValue = 1;
    private Transform goalpos;
    private Animator animator;

    [Header("Movement Parameter")]
    [SerializeField] private float speed = 2.0f;

    [Header("Idle Mode")]
    private float idleDuration;
    private float idleTimer;
    private bool firstTime;
    [SerializeField] private bool idleMode;
    
    private void Awake() 
    {
        animator = GetComponent<Animator>();
        firstTime = true;
    }

    private void Reset() => this.tag = "Enemy";

    void Update()
    {
        goalpos = points[NextLocation];

        if(idleMode)
        {
            if(Vector2.Distance(transform.position, goalpos.position) > 0) MoveToNextPos();
            else if(Vector2.Distance(transform.position, goalpos.position) == 0) ChangeDirectionWithIdle();
        }
        else if(!idleMode)
        {
            if(Vector2.Distance(transform.position, goalpos.position) > 0) MoveToNextPos();
            else if(Vector2.Distance(transform.position, goalpos.position) == 0) ChangeDirectionWithoutIdle();
        }
    }

    private void OnDisable() => animator.SetBool("Move",false);
    
    public void MoveToNextPos()
    {
        if(idleMode) idleTimer = 0;
        
        animator.SetBool("Move", true);

        if(goalpos.transform.position.x > transform.position.x) transform.localScale = new Vector3(1, 1, 1);
        else if(goalpos.transform.position.x <= transform.position.x) transform.localScale = new Vector3(-1, 1, 1);      

        transform.position = Vector2.MoveTowards(transform.position, goalpos.position, speed * Time.deltaTime);
    }

    private void ChangeDirectionWithIdle()
    {
        animator.SetBool("Move", false);

        if(firstTime)
        {
            idleDuration = Random.Range(1.0f,2.0f);
            firstTime = false;
        }

        idleTimer += Time.deltaTime;
                    
        if(idleTimer > idleDuration)
        {
            idleDuration = Random.Range(1.0f,2.0f);

            if(NextLocation == points.Count-1) LocationChangerValue = -1;
            
            if(NextLocation == 0) LocationChangerValue = 1;
            
            NextLocation += LocationChangerValue;
        }
    }

    private void ChangeDirectionWithoutIdle()
    {
        if(NextLocation == points.Count - 1) LocationChangerValue = -1;
        
        if(NextLocation == 0) LocationChangerValue = 1;
        
        NextLocation += LocationChangerValue;
    }
}
