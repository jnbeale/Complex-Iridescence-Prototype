using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public Animator _anim;

    public Transform Player;

    public Transform touchingPlayer;

    private string detectionTag = "Player";

    public bool isTouchingPlayer = false;

     [SerializeField] private LayerMask playerLayer;


    private void awake()
    {
        _anim = GetComponent<Animator>();
    }
    public void TakeDamage (int damage)
    {
        health -= damage;

        if(health <= 0)
        {
             //Die();
            _anim.SetBool("isDead", true);
            Die();
        }
    }


    void Die()
    {
        Destroy(gameObject, 1.5f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance (Player.position, touchingPlayer.position) < 3f)
        {
            _anim.Play("goblin_attack");
        }
        else if (Vector3.Distance (Player.position, touchingPlayer.position) > 3f)
        {
            _anim.Play("goblin_idle");
        }
        //isTouchingPlayer = Physics2D.OverlapCircle(touchingPlayer.position, .2f);

        //if(isTouchingPlayer)
        {
           // _anim.Play("goblin_attack");
        }
    }
     void OnTriggerEnter(Collider other) {
     if (other.tag == "Player")
     {
        _anim.Play("goblin_attack");
     }
 }

}
