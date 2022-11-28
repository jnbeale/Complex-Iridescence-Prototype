using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region serialized
    //health
    private int health = 3;
    //animator
    private Animator _anim;
    //Ai
    public Transform touchingPlayer;
    private GameObject temp;
    private Transform Player;
    private bool isTouchingPlayer = false;




    #endregion

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        temp = GameObject.Find("Player");
        Player = temp.GetComponent<Transform>();
        
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
    }

    //enemy looses health, when health reaches zero Die() function is called
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

    //object is destroyed
    void Die()
    {
        Destroy(gameObject, 1.5f);
    }


    

}
