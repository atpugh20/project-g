using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private float horizontalInput;
    private bool jumpInput;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D fCol;
    // Constants
    private bool onGround = false;
    public float speed = 20f;
    public float jumpVelocity = 35;
    
    
    void Start() {
        // Start is called before the first frame update
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        fCol = GameObject.Find("FeetCollider").GetComponent<BoxCollider2D>();

    }

    private void FixedUpdate() {
        // Runs every 60 frames

        horizontalInput = Input.GetAxisRaw("Horizontal");
        // flip sprite based on movement direction
        if (horizontalInput > 0) {
            sr.flipX = true;
        } else if (horizontalInput < 0) {
            sr.flipX = false;
        }
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);
    }


    void Update() {
        // Runs once every frame (machine dependent)

        jumpInput = Input.GetButton("Jump");
        if (jumpInput && onGround) {
            onGround = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
       
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag == "Ground") onGround = true;
        print(fCol);
    }
}
