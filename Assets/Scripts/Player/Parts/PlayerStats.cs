using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float health {get; private set;}
    private float maxHealth;
    public Slider healthSlider;
    private void Awake() 
    {
        health = 3f;
        maxHealth = health;
        healthSlider.value = 1;
    }

    public void TakeDamage (int damage)
    {
        health -= damage;
        healthSlider.value = health / maxHealth;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
