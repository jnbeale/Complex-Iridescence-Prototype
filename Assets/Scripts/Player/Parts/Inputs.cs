using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    // input action map for controls
    private PlayerInputs _inputs;

    private void Awake()
    {
        _inputs = new PlayerInputs();
    }
    private void OnEnable()
    {
        if (_inputs == null) return;
        // enabling map
        _inputs.Enable();
    }
    private void OnDisable()
    {
        if (_inputs == null) return;
        // disabling map
        _inputs.Disable();
    }
    public void Init(PlayerControler controler)
    {
        // movement methods
        _inputs.Movement.Horizontal.started += ctx => controler._movevement.StartWalking();
        _inputs.Movement.Horizontal.performed += ctx => controler._movevement.Move(ctx.ReadValue<float>());
        _inputs.Movement.Horizontal.canceled += ctx => controler._movevement.StopMovement();
        _inputs.Movement.Jumps.performed += ctx => controler._movevement.CheckJumps();
        _inputs.Movement.Jumps.canceled += ctx => controler._movevement.JumpCancel();
        // combat methods
        _inputs.Movement.Shoot.performed += ctx => controler._combat.Shoot();
        _inputs.Movement.VerticalAim.performed += ctx => controler._combat.Aim(ctx.ReadValue<Vector2>());
        _inputs.Menu.pause.performed += ctx => controler._pause.PauseGame();
    }
}
