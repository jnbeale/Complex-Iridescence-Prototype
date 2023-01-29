using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Moving")]
    [SerializeField] private float enemySpeed;
    private bool movingLeft;



    private Vector3 initScale;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    
    private void Update()
    {
        if(movingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else{
                //Change direction
                DirectionChange();
               
            }
        }
        else
        {
             if(enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                //Change direction
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        movingLeft = !movingLeft;
    }
    private void MoveInDirection(int _direction)
    {

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
         initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * enemySpeed,
         enemy.position.y, enemy.position.z);
    }
}
