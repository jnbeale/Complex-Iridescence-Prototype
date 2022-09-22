using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterController : MonoBehaviour
{

    public float runSpeed = 8f;
    private float horizontal;
    public float jumpingPower = 16f;
    public bool isFacingRight = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    bool isTouchingFront;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlidingSpeed;

    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    bool jump = false;
    public bool isGroundedb;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();


        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            
            jump = true;
        }

        if (Input.GetButtonDown("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.y, rb.velocity.y * 0.5f);
        }

        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, 0.2f, groundLayer);

        if(isTouchingFront == true && isGrounded() == false && horizontal != 0)
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

        if (Input.GetButtonDown("Jump") && wallSliding == true)
        {
            wallJumping = true;
            Invoke("setWallJumpingToFalse", wallJumpTime);
        }

        if(wallJumping)
        {
            rb.velocity = new Vector2(xWallForce * -horizontal, yWallForce);
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

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

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
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
