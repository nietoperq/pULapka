using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float leftCap; //lewa krawedz pola, po ktorym porusza sie wrog
    [SerializeField] private float rightCap; //prawa krawedz pola, po ktorym porusza sie wrog 
    [SerializeField] private LayerMask ground; //warstwa zawierajaca elementy, po ktorych gracz moze chodzic i po ktorych moze skakac

    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource death;

    private float walkingSpeed = 3f; //szybkosc ruchu wroga
    private bool facingLeft = false;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        death = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                //jezeli wrog znajduje sie po prawej stronie lewej krawedzi, to ustawiamy jego sprite w lewa strone i przesuwamy go w lewo
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1); //obrot postaci w lewo
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-walkingSpeed, rb.velocity.y); //ruch postaci w lewo
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {  //jezeli wrog znajduje sie po lewej stronie prawej krawedzi, to ustawiamy jego sprite w prawa strone i przesuwamy go w prawo
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1); //obrot postaci w prawo
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(walkingSpeed, rb.velocity.y); //ruch postaci w prawo
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

    public void JumpedOn()
    {
        //przy skoku na wroga wlaczamy animacje smierci i usuwamy obiekt
        anim.SetTrigger("death");
        rb.velocity = Vector2.zero;
        death.Play();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

}
