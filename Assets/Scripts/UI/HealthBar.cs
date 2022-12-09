using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    // Start is called before the first frame update
    public void SetMaxHealth(int health)
    {
        slider.value = health;
        slider.maxValue = health;

    }
    public void SetHealth(int health)
    {
        slider.value = health;

    }
}
