using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform groundCheckCollider;
    public LayerMask groundLayer;
    Rigidbody2D rb;
    Animator animator;

    [Header("Walk , Jump & Run")]
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    const float groundCheckRadius = 0.2f;
    public int totalJumps;
    int availableJumps;
    [SerializeField] float coyoteTime;
    float horizontalValue;
    [SerializeField] float runSpeedModifier = 2f;
    
    bool isGrounded = true;    
    bool isRunning;
    bool facingRight;
    bool multipleJump;
    bool coyoteJump;
    public bool isAirborne;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        availableJumps = totalJumps;

        if(transform.localScale.x == -2.0f)
        {
            facingRight = false;
        }
        else if(transform.localScale.x == 2.0f)
        {
            facingRight = true;
        }
    }

    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        
        if(Input.GetKeyDown(KeyCode.LeftShift)) isRunning = true;
        
        if(Input.GetKeyUp(KeyCode.LeftShift)) isRunning = false;
        
        if(Input.GetButtonDown("Jump")) Jump();

        if(Mathf.Abs(rb.velocity.y) > 0) isAirborne = true;
        else if(rb.velocity.y == 0) isAirborne = false;
        
        animator.SetFloat("YVelocity",rb.velocity.y);
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue);
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
    
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            }        
        }    
        else
        {
            if (wasGrounded)
            {
                StartCoroutine(CoyoteJumpDelay(coyoteTime));
            }
        }

        animator.SetBool("Jump", !isGrounded);
    }

    #region Jump
    IEnumerator CoyoteJumpDelay(float time)
    {
        coyoteJump = true;
        yield return new WaitForSeconds(time);
        coyoteJump = false;
    }

    void Jump()
    {
        if (isGrounded)
        {
            multipleJump = true;
            availableJumps--;

            rb.velocity = Vector2.up * jumpPower;
            animator.SetBool("Jump", true);
        }
        else
        {
            if(coyoteJump)
            {
                multipleJump = true;
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }

            if(multipleJump && availableJumps>0)
            {
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }
        }
    }
    #endregion

    void Move(float dir)
    {
        #region Move & Run
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
 
        if (isRunning) xVal *= runSpeedModifier;
        
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;
 
        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }

        animator.SetFloat("XVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }  

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
    } 
}
