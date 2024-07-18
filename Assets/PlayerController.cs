using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private float horizontalInput;
    private bool jumpInput;
    private bool boostInput;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EdgeCollider2D ec;
    private BoxCollider2D bc;
    private TrailRenderer tr;
    // Constants
    private bool onGround = false;
    private bool isBoosted = false;
    private float dir = -1f;
    public float speed = 1000f;
    public float jumpVelocity = 35f;
    public float boostStrength = 5f;

    // Before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ec = GetComponent<EdgeCollider2D>();
        tr = GetComponent<TrailRenderer>();
    }

    // Runs every 60 frames
    private void FixedUpdate() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0) {
            sr.flipX = true;
            dir = 1f;
        } else if (horizontalInput < 0) {
            sr.flipX = false;
            dir = -1f;
        }
        float horizontalMovement = horizontalInput * speed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);
    }

    // Runs once every frame (machine dependent)
    void Update() {
        jumpInput = Input.GetButton("Jump");
        boostInput = Input.GetButton("Fire3");
        if (jumpInput && onGround) rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        if (boostInput && !isBoosted) {
            tr.emitting = true;
            isBoosted = true;
            float boostVel = dir * speed * boostStrength * Time.deltaTime;
            rb.velocity = new Vector2(boostVel, jumpVelocity); 
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.otherCollider == ec) { 
            onGround = true;
            isBoosted = false;
            tr.emitting = false;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        onGround = false;
    }
}
