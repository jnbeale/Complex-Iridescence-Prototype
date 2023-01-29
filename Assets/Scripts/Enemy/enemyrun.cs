using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyrun : MonoBehaviour
{
   [Header ("Run Points")]

    [Header("Enemy")]
    [SerializeField] public Transform enemy;

    [Header("Moving")]
    [SerializeField] private float enemySpeed;

    [Header("Player")]
    [SerializeField] public Transform player;
    private bool movingLeft;

    public float attackDistance;
    public float currattackDistance =2;



    public int moveDirection = 1;



    private Vector3 initScale;

    public bool canmove = true;

    private void Awake()
    {
        
        //initScale = enemy.localScale;
        //player = GetComponent<Transform>();
        //enemy = GetComponent<Transform>();
        //CheckRotation();
    }

    
    private void Update()
    {

         
        
        //CheckRotation();
       // attackDistance = AttackDistance();


        
        /*if(attackDistance >= 5)
        {
            canmove = true;
        }
        if(attackDistance >= 1 && canmove == true)
        {
          transform.position += transform.forward * enemySpeed * Time.deltaTime;
        }
        else if (attackDistance < 1)
        {
            canmove = false;
        }*/
        
    }
    private void CheckRotation()
    {
        if(player == null) return;
        if (enemy.position.x < player.position.x)
        {
            moveDirection = 1;
            return;
        }
        else{
            moveDirection = -1;
        }
    }
    private float AttackDistance()
    {
        if(player == null || enemy == null) return currattackDistance + 1;
        return Vector3.Distance(player.position, enemy.position);
    }

     private void MoveInDirection(int _direction)
    {

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
         initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * enemySpeed,
         enemy.position.y, enemy.position.z);
    }
}
