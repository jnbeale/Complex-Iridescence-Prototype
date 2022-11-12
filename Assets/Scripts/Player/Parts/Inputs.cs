using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    // input action map for controls
    private static PlayerInputs _inputs;

    private void OnEnable()
    {
        // enabling map
        _inputs.Enable();
    }
    private void OnDisable()
    {
        // disabling map
        _inputs.Disable();
    }
    public void Init(PlayerControler controler)
    {
        // movement methods
        _inputs = new PlayerInputs();
        _inputs.Movement.Horizontal.started += ctx => controler._movevement.StartWalking();
        _inputs.Movement.Horizontal.performed += ctx => controler._movevement.Move(ctx.ReadValue<float>());
        _inputs.Movement.Horizontal.canceled += ctx => controler._movevement.StopMovement();
        _inputs.Movement.Jumps.performed += ctx => controler._movevement.CheckJumps();
        _inputs.Movement.Jumps.canceled += ctx => controler._movevement.JumpCancel();
        // combat methods
        _inputs.Movement.Shoot.performed += ctx => controler._combat.Shoot();
        _inputs.Movement.VerticalAim.performed += ctx => controler._combat.Aim(ctx.ReadValue<Vector2>());
    }
}
