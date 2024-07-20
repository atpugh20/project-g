using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.5f; // Time delay before the platform starts falling
    [SerializeField] private float fallSpeed = 3f; // Speed at which the platform falls
    [SerializeField] private LayerMask groundLayer; // Layer to detect ground

    private Rigidbody2D _rb;
    private bool _playerUnderneath = false;
    private float _fallTimer = 0f;
    private bool _hasLanded = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true; // Initially, the platform should not be affected by physics
    }

    private void Update()
    {
        if (_playerUnderneath && !_hasLanded)
        {
            _fallTimer += Time.deltaTime;
            if (_fallTimer >= fallDelay)
            {
                // Start falling
                _rb.isKinematic = false;
                _rb.velocity = new Vector2(_rb.velocity.x, -fallSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerUnderneath = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerUnderneath = false;
            _fallTimer = 0f; // Reset timer if player leaves
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            // Check if the platform has landed
            if (!_hasLanded)
            {
                _hasLanded = true;
                _rb.velocity = Vector2.zero; // Stop any remaining movement
                _rb.isKinematic = true; // Stop the platform from falling further
            }
        }
    }
}
