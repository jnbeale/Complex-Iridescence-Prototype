using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    #region private
    // component variables
    private Rigidbody2D _rb;
    private Collider2D _col;
    // movement variables
    private float _horizontal;
    private bool _canMove = true;

    // movevent times for curves
    private float _started;
    private bool _fliping = false;
    //jump variables
    private bool _doubleJumped = false;
    private bool _grounded = false;
    private bool _isJumping = false;
    private bool _isWallJumping = false;
    private float _midAirSpeed = 0;
    private float _counter = 0;

    //animator
    private Animator _anim;
    #endregion
    #region properties
    public Transform _transform { get; set; }
    
    public bool _facingRight { get; private set; }
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
    [Range(0.1f, 6)]
    [SerializeField]
    private float WalkSpeedingTime = 1f;
    [SerializeField]
    private float ChangeDirectionDelay = 0.2f;
    [Header("Jump variables")]
    [Tooltip("Force of player jump")]
    [Range(4, 25)]
    [SerializeField]
    private float JumpForce = 7.5f;
    [Tooltip("Walljump X direction force")]
    [Range(1, 15)]
    [SerializeField]
    private float WallJumpSpeed = 10;
    [Tooltip("How long it takes to reach max height of jump")]
    [Range(0.2f, 15)]
    [SerializeField]
    private float JumpTime = 0.6f;
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
    #endregion

    #region public
    [Header("Gizmos settings")]
    [Tooltip("Displays box for walljump checking, , if enabled error ocurs if game is not running. Dont worry it will disapear after game startd.")]
    public bool drawGizmos = false;
    #endregion

    // used to modify jump force, 
    private const float _JumpMultiplayer = 50;

    private void Awake()
    {
        _facingRight = true;
        // checking grounded
        _grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
        // getting components
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //apply animator 
        _anim.SetFloat("speed", Mathf.Abs(_horizontal));
        //call to jump check function
        isJumping();
        CheckingGround();
        JumpCounting();
        if (!_canMove)
        {
            return;
        }
        Move();
    }

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
        if (!Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer))
        {
            if (_grounded)
            {
                _grounded = false;
                if (_horizontal != 0)
                {
                    _midAirSpeed = CheckSpeed();
                }
                else
                {
                    _midAirSpeed = 0;
                }
            }
            return;
        }
        if (!_grounded)
        {
            _started = Time.time - (WalkSpeedingTime * 0.5f);
        }
        _doubleJumped = false;
        _grounded = true;
    }
    #endregion
    #region Movement methods
    public void Move(float direction)
    {
        if (direction == -_horizontal)
        {
            StartCoroutine(ChangeDirectionWait());
        }
        _horizontal = direction;
    }
    public void StopMovement()
    {
        _horizontal = 0;
    }
    // reseting time when player stopped
    public void StartWalking()
    {
        _started = Time.time;
    }
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
        if (!_grounded)
        {
            return _midAirSpeed;
        }
        if (_fliping)
        {
            return 0;
        }
        return Speed * WalkingCurve.Evaluate(((Time.time - _started) / WalkSpeedingTime));
    }
    private IEnumerator ChangeDirectionWait()
    {
        _fliping = true;
        yield return new WaitForSeconds(ChangeDirectionDelay);
        _started = Time.time;
        _fliping = false;
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
    // calculating diferent jump heights
    private void JumpCounting()
    {
        if (_isJumping && _counter < JumpTime) // for normal jump
        {
            _counter += Time.fixedDeltaTime;
            Vector2 direction = _rb.velocity;
            direction.y = JumpForce * Time.fixedDeltaTime * _JumpMultiplayer;
            _rb.velocity = direction;
        }
        else if (_isWallJumping && _counter < JumpTime) // for wall jump
        {
            _counter += Time.fixedDeltaTime;
            Vector2 direction = _rb.velocity;
            direction.y = JumpForce * Time.fixedDeltaTime * _JumpMultiplayer;
            direction.x = _facingRight ? WallJumpSpeed : -WallJumpSpeed;
            _rb.velocity = direction;
        }
        else // for reseting
        {
            _counter = 0;
            _isJumping = false;
            _isWallJumping = false;
        }
    }
    public void CheckJumps()
    {
        // check for wall jump
        if (CheckWall() && !_grounded)
        {
            WallJump(); // wall jump
            return;
        }
        if (!_grounded && _doubleJumped) return; // check if player can jump or double jump
        if (_grounded)
        {
            _midAirSpeed = CheckSpeed();
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
        _isJumping = true;
    }

    //function to check if player is jumping
    public void isJumping()
    {
        if (!_grounded)
        {
            _anim.SetBool("isJumping", true);
        }
        else
        {
            _anim.SetBool("isJumping", false);
        }
    }

    private void WallJump()
    {
        _isWallJumping = true;
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
    public void JumpCancel()
    {
        _isJumping = false;
        _isWallJumping = false;
        _counter = 0;
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
