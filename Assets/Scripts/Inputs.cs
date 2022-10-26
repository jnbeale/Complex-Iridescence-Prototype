using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    // input action map for controls
    private static PlayerInputs _inputs;

    private void Awake()
    {
        // creating map
        _inputs = new PlayerInputs();
    }

    public static PlayerInputs GetInputs()
    {
        return _inputs;
    }

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
}
