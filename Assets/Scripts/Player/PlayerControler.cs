using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inputs))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Combat))]
public class PlayerControler : MonoBehaviour
{
    public Inputs _inputs { get; private set; }
    public Movement _movevement { get; private set; }
    public Combat _combat { get; private set; }
    public Transform _playerTransform { get; private set; }
    private void Awake()
    {
        //getting componets
        _playerTransform = transform;
        _inputs = GetComponent<Inputs>();
        _movevement = GetComponent<Movement>();
        _combat = GetComponent<Combat>();
        // setting parameters
        _inputs.Init(this);
        _movevement._transform = _playerTransform;
        _combat._movement = _movevement;
    }
}