using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health {get; private set;}
    private void Awake() 
    {
        health = 3f;    
    }

    public void TakeDamage (int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
