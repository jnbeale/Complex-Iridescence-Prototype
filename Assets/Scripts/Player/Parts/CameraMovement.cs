using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTrans;

    //camera transforms
    private Transform _camTransform;
    private Vector3 _ofset;

    void Awake()
    {
        _camTransform = transform;
        _ofset = _camTransform.position - playerTrans.position;
    }

    void LateUpdate() // runs after update, moves camera after player moved
    {
        if(playerTrans == null) return;
        _camTransform.position = playerTrans.position + _ofset;
    }
}
