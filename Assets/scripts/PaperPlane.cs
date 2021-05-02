using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : MonoBehaviour
{

    [SerializeField] private float leftCorner;
    [SerializeField] private float rightCorner;

    private Rigidbody2D rb;

    private float flyingSpeed = 8f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > leftCorner)
        {
            rb.velocity = new Vector2(-flyingSpeed, 0); //ruch postaci w lewo
        }
        else
        {
            rb.position = new Vector2(rightCorner, rb.position.y);
        }

    }

}
