using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        //jezeli obiekt z przylaczonym skryptem HurtPlayer wejdzie w kolizje z obiektem o tagu "player", a wiec graczem, to odejmujemy zycie 
        if (other.gameObject.tag == "player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            //jezeli gracz zebral ulepszenie zapewniajace niesmiertelnosc i jest ono aktywne, to nie odejmujemy zycia
            if (!player.getImmortalInfo())
            {
                player.setState(4); //state 4 = hurt
                player.Damage(damage);
            }
        }
    }

}
