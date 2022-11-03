using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Inputs))]
public class Movement : MonoBehaviour
{
    #region private
    // component variables
    private Transform _transform;
    private Rigidbody2D _rb;
    private Collider2D _col;
    // movement variables
    private float _horizontal;
    private bool _facingRight = true;
    private bool _canMove = true;

    // movevent times for curves
    private float _started;
    //jump variables
    private bool _doubleJumped = false;
    private bool _grounded = false;
    #endregion

    #region serialized 
    [Header("Walking variables")]
    [Tooltip("Speed of player")]
    [Range(0.5f, 10f)]
    [SerializeField]
    private float Speed = 1f;
    [Tooltip("Curve of walking speed")]
    [SerializeField]
    private AnimationCurve WalkingCurve;
    [Tooltip("Time how long it will take to reach max speed in seconds")]
    [Range(0.1f,6)]
    [SerializeField]
    private float WalkSpeedingTime = 1f;
    [Header("Jump variables")]
    [Tooltip("Force of player jump")]
    [Range(250, 2000)]
    [SerializeField]
    private float JumpForce = 800;
    [Tooltip("Walljump X direction force")]
    [Range(250, 1600)]
    [SerializeField]
    private float WallJumpXForce = 600f;
    [Tooltip("GroundCheck transform")]
    [SerializeField]
    private Transform GroundCheck;
    [Tooltip("Radius of groundcheck function")]
    [Range(0.1f, 0.7f)]
    [SerializeField]
    private float GroundRadius = 0.5f;
    [Tooltip("Layer of ground tiles")]
    [SerializeField]
    private LayerMask GroundLayer;

    //animator
    public Animator anim;
    #endregion

    #region public
    [Header("Gizmos settings")]
    [Tooltip("Displays box for walljump checking, , if enabled error ocurs if game is not running. Dont worry it will disapear after game startd.")]
    public bool drawGizmos = false;
    #endregion

    private void Awake()
    {
        // checking grounded
        _grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
        // getting components
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        SetUpInputs();
    }
    private void FixedUpdate()
    {
        //animator 
        anim.SetFloat("speed",Mathf.Abs(_horizontal));
        CheckingGround();
        // aplying physics
        if (!_canMove)
        {
            return;
        }
        Move();
    }

    #region Input Handling
    private void SetUpInputs()
    {
        PlayerInputs _inputs = Inputs.GetInputs();
        //setting up c# events for input control
        _inputs.Movement.Horizontal.started += ctx => StartWalking();
        _inputs.Movement.Horizontal.performed += ctx => Move(ctx.ReadValue<float>());
        _inputs.Movement.Horizontal.canceled += ctx => _horizontal = 0;
        _inputs.Movement.Jumps.performed += ctx => CheckJumps();
    }
    private void Move(float direction)
    {
        _horizontal = direction;
    }
    #endregion
    #region Checking
    //checking for wall
    private bool CheckWall()
    {
        Vector3 center = _col.bounds.center; // center of collider
        Vector2 size = _col.bounds.extents; // size of collider
        size.y += 0.05f; // make smaller vertical size of box size
        size.x += 0.2f; // make smaller harizontal size of box size
        // checking to right direction using boxCast
        if (_facingRight)
        {
            center.x += 0.3f; // moving center of box for boxcast based on facing direction
            return Physics2D.BoxCast(center, size, 0, Vector2.zero, 0, GroundLayer);
        }
        center.x -= 0.3f; // moving center of box for boxcast based on facing direction
        return Physics2D.BoxCast(center, size, 0, Vector2.zero, 0, GroundLayer);
    }
    // checking if player is grounded
    private void CheckingGround()
    {
        _grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
        if (_grounded)
        {
            _doubleJumped = false;
        }
    }
    #endregion
    #region Movement methods
    private void Move()
    {
        Flip(); // check rotation
        Vector2 direction = _rb.velocity;
        if (CheckWall())
        {
            direction = _rb.velocity;
            direction.x = 0;
            _rb.velocity = direction;
            return;
        }
        direction = _rb.velocity;
        direction.x = _horizontal * CheckSpeed();
        _rb.velocity = direction;
    }
    // return speed based on curve value
    private float CheckSpeed()
    {
        return Speed * WalkingCurve.Evaluate(((Time.time - _started)/WalkSpeedingTime));
    }
    // reseting time when player stopped
    private void StartWalking()
    {
        _started = Time.time;
    }
    // update player direction
    private void Flip()
    {
        if (_horizontal == 0) return;
        // checking direction from input value
        if (_horizontal > 0)
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0); // changing rotation
            _facingRight = true;
            return;
        }
        _transform.rotation = Quaternion.Euler(0, 180, 0); // changing rotation
        _facingRight = false;
    }
    #endregion
    #region Jump methods
    private void CheckJumps()
    {
        // check for wall jump
        if (CheckWall() && !_grounded)
        {
            WallJump(); // wall jump
            return;
        }
        if (!_grounded &&  _doubleJumped) return; // check if player can jump or double jump
        if (_grounded)
        {
            Jump(); // base jump on ground
            return;
        }
        if (!_doubleJumped)
        {
            _doubleJumped = true;
            _canMove = true;
            _rb.velocity = Vector2.zero;
            Jump(); // double jump
        }

    }
    public void Jump()
    {
        Vector2 direction = _rb.velocity;
        direction.y = 0; // setting velocity to zero
        _rb.velocity = direction;
        _rb.AddForce(new Vector2(0, JumpForce)); // adding force
    }

    private void WallJump()
    {
        Vector2 direction = _rb.velocity;
        direction = Vector2.zero;
        _rb.velocity = direction; // setting velocity to zero
        Vector2 force = new Vector2(WallJumpXForce, JumpForce);
        if (_facingRight) // checking for wall jump direction
        {
            force.x = -force.x; 
        }
        _rb.AddForce(force); // adding force
        _facingRight = !_facingRight;
        WallJumpFlip(); // changing rotation after wall jump
        StartCoroutine(WallJumpWait()); // waiting for player to touch ground or hit next wall for wall jump
    }
    private void WallJumpFlip()
    {
        // rotates player to oposite direction
        if (_facingRight)
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        _transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    
    private IEnumerator WallJumpWait()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        _canMove = false;
        while (!_grounded)
        {
            // when player hits ground loop stop
            CheckingGround();
            yield return wait; // wait for end of current frame, game wont crash even in infinite loop
        }
        _rb.velocity = Vector2.zero;
        _canMove = true;
    }
    #endregion
    #region Gizmos
    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        // displays box for wall checking
        Vector3 center = _col.bounds.center;
        Vector2 size = _col.bounds.extents;
        Color color = Color.magenta;
        size.y += 0.05f;
        size.x += 0.02f;
        if (CheckWall())
        {
            color = Color.green; // color green if wall is detected
        }
        if (_facingRight)
        {
            center.x += 0.3f;
        }
        else
        {
            center.x -= 0.3f;
        }
        Debug.DrawLine(new Vector2(center.x + size.x, center.y + size.y), new Vector2(center.x + size.x, center.y - size.y), color);
        Debug.DrawLine(new Vector2(center.x - size.x, center.y + size.y), new Vector2(center.x - size.x, center.y - size.y), color);
        Debug.DrawLine(new Vector2(center.x - size.x, center.y + size.y), new Vector2(center.x + size.x, center.y + size.y), color);
        Debug.DrawLine(new Vector2(center.x + size.x, center.y - size.y), new Vector2(center.x - size.x, center.y - size.y), color);
    }
    #endregion
}
