using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
    #region gizmos variables
    private int _zone = -1;
    private Color[] _colors;
    #endregion
    public Camera cam;
    public Bullet bullet;

    private Movement _movement;
    private Vector2 _aimVector;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _colors = new Color[8];
        for (int i = 0; i < 8; i++)
        {
            _colors[i] = Color.red;
        }
    }
    private void Start()
    {
        PlayerInputs input = Inputs.GetInputs();
        input.Movement.Shoot.performed += ctx => Shoot();
        input.Movement.VerticalAim.performed += ctx => Aim(ctx.ReadValue<Vector2>());
    }
    private void Update()
    {
        CalculateZone();
        ColorCheck();
    }
    private void Aim(Vector2 aimVector)
    {
        _aimVector = aimVector;
    }
    #region zone calculation
    private void CalculateZone()
    {
        if (_movement._facingRight)
        {
            RightAim(_aimVector);
            return;
        }
        LeftAim(_aimVector);

    }
    private void RightAim(Vector2 aimDirection)
    {
        if(aimDirection.x < 0 || aimDirection.x == 0)
        {
            if(aimDirection.y < 0) 
            {
                _zone = 6;
            }
            else if (aimDirection.y > 0) 
            {
                _zone = 2;
            }
            else
            {
                _zone = 0;
            }
        }else
        {
            if (aimDirection.y < 0)
            {
                _zone = 7;
            }
            else if (aimDirection.y > 0)
            {
                _zone = 1;
            }
            else
            {
                _zone = 0;
            }
        }
    }
    private void LeftAim(Vector2 aimDirection)
    {
        if (aimDirection.x > 0 || aimDirection.x == 0)
        {
            if (aimDirection.y < 0)
            {
                _zone = 6;
            }
            else if (aimDirection.y > 0)
            {
                _zone = 2;
            }
            else
            {
                _zone = 4;
            }
        }
        else
        {
            if (aimDirection.y < 0)
            {
                _zone = 5;
            }
            else if (aimDirection.y > 0)
            {
                _zone = 3;
            }
            else
            {
                _zone = 4;
            }
        }
    }
    #endregion
    private void Shoot()
    {
        Bullet shot = Instantiate(bullet);
        shot.transform.position = transform.position;
        shot.Init(RotateVector(Vector2.right, 45 * _zone), 45f * _zone);
    }
    public Vector2 RotateVector(Vector2 v, float degree)
    {
        float delta = Mathf.Deg2Rad * degree;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    #region gizmos
    private void ColorCheck()
    {
        Color aColor = Color.green;
        for (int i = 0; i < 8; i++)
        {
            _colors[i] = Color.red;
        }
        switch (_zone)
        {
            case 0:
                _colors[0] = aColor;
                _colors[7] = aColor;
                break;
            case 1:
                _colors[0] = aColor;
                _colors[1] = aColor;
                break;
            case 2:
                _colors[1] = aColor;
                _colors[2] = aColor;
                break;
            case 3:
                _colors[2] = aColor;
                _colors[3] = aColor;
                break;
            case 4:
                _colors[3] = aColor;
                _colors[4] = aColor;
                break;
            case 5:
                _colors[4] = aColor;
                _colors[5] = aColor;
                break;
            case 6:
                _colors[5] = aColor;
                _colors[6] = aColor;
                break;
            case 7:
                _colors[6] = aColor;
                _colors[7] = aColor;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying) return;
        for (int i = 0; i < 8; i++)
        {
            Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + RotateVector(Vector2.right, 22.5f * (i * 2 + 1)) * 2, _colors[i]);
        }
    }
    #endregion
}
