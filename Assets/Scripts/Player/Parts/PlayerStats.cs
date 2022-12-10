using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    
    public float health {get; private set;}

    public SimpleFlash _flash {get; private set;}

    public SpriteRenderer playerSprite;
    private float maxHealth;
    public Slider healthSlider;

    public bool canTakeDamage;
    private void Awake() 
    {
        _flash = GetComponent<SimpleFlash>();
        health = 3f;
        maxHealth = health;
        healthSlider.value = 1;
        canTakeDamage = true;
    }

public void TakeDamage (int damage)
    {
        
        if(canTakeDamage == true){
        health -= damage;
        _flash.Flash();
        StartCoroutine(invincibility());
        healthSlider.value = health / maxHealth;
        if(health <= 0)
        {
            Destroy(this.gameObject);
            Application.Quit();
        }
        }
    }

    IEnumerator  invincibility()
{
    canTakeDamage = false;
    _flash.Flash();
    yield return new WaitForSeconds(.2f);
    _flash.Flash();
    yield return new WaitForSeconds(.2f);
    _flash.Flash();
    yield return new WaitForSeconds(.2f);
    _flash.Flash();
    yield return new WaitForSeconds(.2f);
    _flash.Flash();
    yield return new WaitForSeconds(.2f);
    canTakeDamage = true;
}


}
