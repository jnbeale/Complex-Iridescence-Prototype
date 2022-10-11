using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
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
    //jump variables
    private bool _jumped = false;
    private bool _doubleJumped = false;
    private bool _grounded = false;
    #endregion

    #region serialized 
    [Header("Walking variables")]
    [Tooltip("Speed of player")]
    [Range(0.5f, 10f)]
    [SerializeField]
    private float Speed = 1f;

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
    #endregion

    #region public
    [Header("Gizmos settings")]
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
    private void Update()
    {
        // checking for inputs
        _horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckJumps();
        }
    }
    private void FixedUpdate()
    {
        // aplying physics
        if (!_canMove)
        {
            return;
        }
        Move();
    }

    //checking for wall
    private bool CheckWall()
    {
        Vector3 center = _col.bounds.center;
        Vector2 size = _col.bounds.extents;
        size.y += 0.05f;
        size.x += 0.2f;
        if (_facingRight)
        {
            center.x += 0.3f;
            return Physics2D.BoxCast(center, size, 0, Vector2.zero, 0, GroundLayer);
        }
        center.x -= 0.3f;
        return Physics2D.BoxCast(center, size, 0, Vector2.zero, 0, GroundLayer);
    }

    #region Movement methods

    private void Move()
    {
        Flip();
        Vector2 direction = _rb.velocity;
        if (CheckWall())
        {
            direction = _rb.velocity;
            direction.x = 0;
            _rb.velocity = direction; 
            return;
        }
        direction = _rb.velocity;
        direction.x = _horizontal * Speed;
        _rb.velocity = direction;
    }
    private void Flip()
    {
        if (_horizontal == 0) return;
        if (_horizontal > 0)
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0);
            _facingRight = true;
            return;
        }
        _transform.rotation = Quaternion.Euler(0, 180, 0);
        _facingRight = false;
    }
    #endregion
    #region Jump methods
    private void CheckingGround()
    {
        _grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
        if (_grounded)
        {
            _jumped = false;
            _doubleJumped = false;
        }
    }
    private void WallJump()
    {
        Vector2 direction = _rb.velocity;
        direction = Vector2.zero;
        _rb.velocity = direction;
        Vector2 force = new Vector2(WallJumpXForce, JumpForce);
        if (_facingRight)
        {
            force.x = -force.x;
        }
        _rb.AddForce(force);
        _facingRight = !_facingRight;
        WallJumpFlip();
        StartCoroutine(WallJumpWait());
    }
    private void WallJumpFlip()
    {
        if (_facingRight)
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        _transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    private void CheckJumps()
    {
        CheckingGround();
        if (CheckWall() && !_grounded)
        {
            WallJump();
            return;
        }
        if (!_grounded && (_jumped && _doubleJumped)) return;
        if (!_jumped)
        {
            _jumped = true;
            Jump();
            return;
        }
        if (!_doubleJumped)
        {
            StopCoroutine(WallJumpWait());
            _doubleJumped = true;
            _canMove = true;
            _rb.velocity = Vector2.zero;
            Jump();
        }

    }
    public void Jump()
    {
        Vector2 direction = _rb.velocity;
        direction.y = 0;
        _rb.velocity = direction;
        _rb.AddForce(new Vector2(0, JumpForce));
    }

    private IEnumerator WallJumpWait()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        _canMove = false;
        while (!_grounded)
        {
            CheckingGround();
            yield return wait;
        }
        _rb.velocity = Vector2.zero;
        _canMove = true;
    }
    #endregion
    #region Gizmos
    private void OnDrawGizmos()
    {
        if(!drawGizmos) return;
        Vector3 center = _col.bounds.center;
        Vector2 size = _col.bounds.extents;
        Color color = Color.magenta;
        size.y += 0.05f;
        size.x += 0.02f;
        if(CheckWall())
        {
            color = Color.green;
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
