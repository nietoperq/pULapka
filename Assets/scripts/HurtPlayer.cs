using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (!player.getImmortalInfo())
            {
                player.setState(4); //state 4 = hurt
                player.Damage(damage);
            }
        }
    }

}
