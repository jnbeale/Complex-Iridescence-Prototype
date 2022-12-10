using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    
    public float health {get; private set;}

    public SpriteRenderer playerSprite;
    private float maxHealth;
    public Slider healthSlider;

    public bool canTakeDamage;
    private void Awake() 
    {
        health = 3f;
        maxHealth = health;
        healthSlider.value = 1;
        canTakeDamage = true;
    }

public void TakeDamage (int damage)
    {
        
        if(canTakeDamage == true){
        health -= damage;
        StartCoroutine(Fade());
        healthSlider.value = health / maxHealth;
        if(health <= 0)
        {
            Destroy(this.gameObject);
            Application.Quit();
        }
        }
    }

    IEnumerator Fade()
{
    canTakeDamage = false;
    for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
    {
        playerSprite.color = new Color(0f, 0f, 0f);
        yield return new WaitForSeconds(.1f);
        playerSprite.color = Color.white;
        yield return new WaitForSeconds(.1f);
        playerSprite.color = new Color(0f, 0f, 0f);
        yield return new WaitForSeconds(.1f);
        playerSprite.color = Color.white;
        playerSprite.color = new Color(0f, 0f, 0f);
        yield return new WaitForSeconds(.1f);
        playerSprite.color = Color.white;
        yield return new WaitForSeconds(3f);
        canTakeDamage = true;
    }
}


}
