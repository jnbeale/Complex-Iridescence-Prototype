using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inputs))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Combat))]
public class PlayerControler : MonoBehaviour
{
    public Inputs _inputs { get; private set; }
    public PlayerStats _stats {get; private set;}
    public Movement _movevement { get; private set; }
    public Combat _combat { get; private set; }
    public Transform _playerTransform { get; private set; }
    public static PlayerControler _instance {get; private set;}

    public pauseMenu _pause { get; private set; }
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
        //getting componets
        _playerTransform = transform;
        _inputs = GetComponent<Inputs>();
        _movevement = GetComponent<Movement>();
        _combat = GetComponent<Combat>();
        _stats = GetComponent<PlayerStats>();
        _pause = GetComponent<pauseMenu>();
        // setting parameters
        _inputs.Init(this);
        _movevement._transform = _playerTransform;
    }
}