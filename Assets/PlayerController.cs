using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 20f;
    private float horizontalInput;
    private bool jumpInput;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isJumping = false;
    
    void Start() {
        // Start is called before the first frame update

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        // Runs every 60 frames

        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0) {
            sr.flipX = true;
        }
        else if (horizontalInput < 0) {
            sr.flipX = false;
        }
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);
    }


    void Update() {
        // Runs once every frame (machine dependent)

        jumpInput = Input.GetButton("Jump");
        if (jumpInput && !isJumping)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 35);
        }
        if (rb.velocity.y == 0) isJumping = false; 
    }
}
