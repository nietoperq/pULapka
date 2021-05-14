using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetHealth(int health)
    {
        //ustawienie wartosci paska zycia na wartosc zycia gracza
        slider.value = health;
    }
}
