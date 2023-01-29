using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_one : MonoBehaviour
{
    private patrol enemyPatrol;

    private enemyrun enemyRun;

    private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public Transform enemy;

    public float cooldownTimer = Mathf.Infinity;

    public float colliderDistance;

    public bool ispatrolling = true;
    public bool isrunning = true;

    public float range;
    [SerializeField] public Transform player;
    Rigidbody2D rb;
    [SerializeField] private float enemySpeed;

    private Vector2 target;

    public GameObject enemyobj;

    public GameObject playerobj;

    

    [SerializeField] public float attackCooldown;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<patrol>();
        enemyRun = GetComponentInParent<enemyrun>();
        player = GetComponent<Transform>();
        enemy = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        target =  new Vector2(0,0);
    }

    private void Update()
    {

        target = new Vector2(playerobj.transform.position.x,-1f);
        float step = enemySpeed * Time.deltaTime;
         

        //cooldownTimer += Time.deltaTime;
        //ispatrolling = true;
        //isrunning = false;
        //enemyRun.enabled = false;
        //enemyPatrol.enabled = false;
        //Vector2 tempVector2 = Vector2.MoveTowards(enemy.position, target, Time.deltaTime * enemySpeed);
         //transform.position = new Vector3(tempVector2.x, tempVector2.y, transform.position.z);
        

        if(PlayerInSight())
        {

            //enemyobj.transform.position = Vector2.MoveTowards(enemy.transform.position, target, step);
            transform.Translate(Vector3.right * enemySpeed * Time.deltaTime);
           
            if(enemyPatrol.enabled == true)
            {
                enemyRun.enabled = false;
                ispatrolling = true;
                isrunning = false;
            }
            if(enemyPatrol.enabled == false)
            {
                enemyRun.enabled = true;
                
                ispatrolling = false;
                isrunning = true;
            }

           /* if(cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("")
            }*/
            if(enemyPatrol != null && enemyRun)
            {
                
                enemyPatrol.enabled = !PlayerInSight();
                //enemyRun.enabled = PlayerInSight();
                 
            }
            

        }
        else
        {
            enemyPatrol.enabled = true;
            ispatrolling = true;
            isrunning = false;
        }

    }
    
     private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }



}

