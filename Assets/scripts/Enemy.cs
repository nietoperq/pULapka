using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private LayerMask ground;
 
    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource death;

    private float walkingSpeed = 3f;
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
        {
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
        anim.SetTrigger("death");
        rb.velocity = Vector2.zero;
        death.Play();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

}
