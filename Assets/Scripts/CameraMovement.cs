using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Player Transform")]
    public Transform player;

    //camera transforms
    private Transform _camTransform;
    private Vector3 _ofset;

    void Awake()
    {
        _camTransform = transform;
        _ofset = _camTransform.position - player.position;
    }

    void LateUpdate()
    {
        _camTransform.position = player.position + _ofset;
    }
}
