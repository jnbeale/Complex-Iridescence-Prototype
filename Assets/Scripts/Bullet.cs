using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _trans;
    private Vector3 _direction = Vector3.zero;
    public float speed = 3f;
    private void Awake()
    {
        _trans = transform;
    }
    private void FixedUpdate()
    {
        if (_direction == Vector3.zero) return;
        _trans.position += _direction * speed * Time.fixedDeltaTime;
    }
    public void Init(Vector2 direction, float angle)
    {
        _direction = new Vector3(direction.x, direction.y, 0);
        _trans.rotation = Quaternion.Euler(0, 0, angle);
        Destroy(this.gameObject, 3);
    }
}
