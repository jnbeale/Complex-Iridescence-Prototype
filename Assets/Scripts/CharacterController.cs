using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float runSpeed = 8f;
    private float horizontal;
    public float jumpingPower = 16f;
    public bool isFacingRight = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    public bool isTouchingFront;
    public Transform frontCheck;
    public bool wallSliding;
    public float wallSlidingSpeed;

    public bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    bool jump = false;
    public bool isGroundedb;

    public LayerMask whatIsGround;

    public float checkRadius;
    public bool isGrounded;

    private bool doubleJump;

   

    // Update is called once per frame
    void Update()
    {
         isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);

        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        if(isGrounded && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        if (Input.GetButtonDown("Jump") || wallSliding && Input.GetButtonDown("Jump"))
        {
            if(isGrounded || doubleJump|| wallSliding)
            {
                 rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                 jump = true;
                 doubleJump = !doubleJump;
            }
            
        }
        else
        {
            jump = false;
        }

        if (Input.GetButtonDown("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.y, rb.velocity.y * 0.5f);
        }

        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, 0.2f, groundLayer);
        
        //added velocity condition --jnbeale
        if(isTouchingFront == true && isGrounded == false && horizontal != 0 && (Input.GetKey("d") || Input.GetKey("a") ))//rb.velocity.x >= 0f)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if(wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (Input.GetButtonDown("Jump") && wallSliding == true && !isGrounded )
        {
            wallJumping = true;
            Invoke("setWallJumpingToFalse", wallJumpTime);
        }

        if(wallJumping == true)
        {
            if(isFacingRight == true){

                rb.velocity = new Vector2(runSpeed * -1, jumpingPower);
            }
            else
            {
                rb.velocity = new Vector2(runSpeed * 1, jumpingPower);
            }

        }

    }

    void setWallJumpingToFalse()
    {
        wallJumping = false;
    }

    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    //private bool isGrounded()
    //{
      //  return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    //}

    public void OnLanding()
    {
        
        jump = false;
    }
    void FixedUpdate()
    {
        //Move Character
        rb.velocity = new Vector2(horizontal * runSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal > 0f || !isFacingRight && horizontal < 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
