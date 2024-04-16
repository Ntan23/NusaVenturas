using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour,IData
{
    #region Variables
    [Header("References")]
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform indicatorText;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("For Movement")]
    [SerializeField] private float speed;
    private float jumpPower;
    const float groundCheckRadius = 0.2f;
    [SerializeField] private int totalJumps;
    int availableJumps;
    [SerializeField] private float coyoteTime;
    private float horizontalValue;
    [SerializeField] private float runSpeedModifier = 2f;

    [Header("For Knockback")]
    [SerializeField] private float knockbackForce;
    private float knockbackCounter;
    [SerializeField] private float knockbackTime;
    private bool knockFromLeft;
    private bool isGrounded = true;    
    private bool isRunning;
    private bool facingRight = true;
    private bool multipleJump;
    private bool coyoteJump;
    private bool isAirborne;
    private bool canControl;
    private PlayerInteraction playerInteraction;
    private GameManager gm;
    private MainMenuManager mm;
    private AudioManager am;
    #endregion

    void Start()
    {
        gm = GameManager.instance;
        mm = MainMenuManager.instance;
        am = AudioManager.instance;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();

        availableJumps = totalJumps;
    }

    void Update()
    {
        if(playerInteraction.GetIsExamining()) 
        {
            horizontalValue = 0;
            return;
        }
        
        horizontalValue = Input.GetAxisRaw("Horizontal");

        if(gm != null)
        {
            canControl = gm.GetCanControl();

            if(!canControl) 
            {
                horizontalValue = 0;
                return;
            }
        }

        if(mm != null) 
        {
            canControl = mm.GetCanControl();

            if(!canControl) 
            {
                horizontalValue = 0;
                return;
            }
        }
         
        if(Input.GetKeyDown(KeyCode.LeftShift) && canControl) isRunning = true;
        
        if(Input.GetKeyUp(KeyCode.LeftShift) && canControl) isRunning = false;
        
        if(Input.GetButtonDown("Jump") && canControl) Jump();

        if(Mathf.Abs(rb.velocity.y) > 0) isAirborne = true;
        else if(rb.velocity.y == 0) isAirborne = false;
        
        animator.SetFloat("YVelocity",rb.velocity.y);
    }

    void FixedUpdate()
    {
        GroundCheck();

        if(knockbackCounter <= 0) 
        {
            canControl = true;

            Move(horizontalValue);
        }
        else if(knockbackCounter > 0)
        {
            canControl = false;

            if(knockFromLeft) rb.velocity = new Vector2(knockbackForce, knockbackForce);
            if(!knockFromLeft) rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        
            knockbackCounter -= Time.deltaTime;
        }
    }

    public void LoadData(GameData gameData) => this.jumpPower = gameData.jumpPower;
    
    public void SaveData(GameData gameData)
    {
        
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
    
        Collider2D collider = Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);
        
        if(collider != null)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            } 

            if(collider.CompareTag("MovingPlatform")) transform.parent = collider.transform;       
        }    
        else
        {
            transform.parent = null;

            if (wasGrounded) StartCoroutine(CoyoteJumpDelay(coyoteTime));
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

            am.Play("Jump");
        }
        else
        {
            if(coyoteJump)
            {
                multipleJump = true;
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);

                am.Play("Jump");
            }

            if(multipleJump && availableJumps > 0)
            {
                availableJumps--;

                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);

                am.Play("Jump");
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
            indicatorText.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            indicatorText.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }

        animator.SetFloat("XVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }  
    
    public void SetKnockback() => knockbackCounter = knockbackTime;
    public void SetKnockFromLeftValue(bool value) => knockFromLeft = value;
    public void UpdateJumpPower(float value) => jumpPower = value;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
    } 

    public bool GetIsAirborne()
    {
        return isAirborne;
    }
}
