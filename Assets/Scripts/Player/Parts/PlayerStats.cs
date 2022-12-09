using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health {get; private set;}

    public HealthBar _healthbar {get; private set;}
    private void Awake() 
    {
        _healthbar = GetComponent<HealthBar>();
        health = 3;
        _healthbar.SetMaxHealth(health);    
    }

public void TakeDamage (int damage)
    {
        health -= damage;
        _healthbar.SetHealth(health);

        if(health <= 0)
        {
            Destroy(this.gameObject);
            Application.Quit();
        }
    }
}
