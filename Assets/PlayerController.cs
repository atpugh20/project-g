using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private float horizontalInput;
    private bool jumpInput;
    private bool boostInput;
    private Rigidbody2D rb;
    //private SpriteRenderer sr;
    private CapsuleCollider2D[] capsules;
    private CapsuleCollider2D fCol;
    private CapsuleCollider2D wCol;
    private BoxCollider2D bc;
    private TrailRenderer tr;
    private Animator animator;
    public Transform transform;
    // Constants
    private bool onGround = false;
    private bool isBoosted = false;
    //private float dir = -1f;
    public float speed = 1000f;
    public float jumpVelocity = 35f;
    public float boostStrength = 5f;
    public float deadZone = 0.05f;

    // Before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        capsules = GetComponents<CapsuleCollider2D>();
        wCol = capsules[0];
        fCol = capsules[1];
        
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    // Runs every 60 frames
    private void FixedUpdate() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        print(horizontalInput);
        if (-deadZone < horizontalInput && horizontalInput < deadZone) {
            animator.speed = 1;
            animator.SetBool("isRunning", false);
        } else {
            animator.speed = Math.Abs(horizontalInput);
            animator.SetBool("isRunning", true);
        }
        if (horizontalInput > 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (horizontalInput < 0) {
            transform.eulerAngles = new Vector3(0, 180, 0);
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
            // float boostVel = dir * speed * boostStrength * Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity); 
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.otherCollider == fCol) {
            onGround = true;
            isBoosted = false;
            tr.emitting = false;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        onGround = false;
    }
}
