using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.5f; // Time delay before the platform starts falling
    [SerializeField] private float fallSpeed = 3f; // Speed at which the platform falls

    private Rigidbody2D _rb;
    private bool _playerUnderneath = false;
    private bool _hasLanded = false;
    private Coroutine fallCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true; // Initially, the platform should not be affected by physics
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_hasLanded)
        {
            Debug.Log("Player entered trigger");
            _playerUnderneath = true;
            if (fallCoroutine == null)
            {
                fallCoroutine = StartCoroutine(StartFalling());
            }
        }
    }

    private IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(fallDelay);
        Debug.Log("Platform starts falling");

        // Start falling
        _rb.isKinematic = false;
        _rb.velocity = new Vector2(_rb.velocity.x, -fallSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name + " with tag " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            // Check if the platform has landed
            if (!_hasLanded)
            {
                Debug.Log("Platform landed");
                _hasLanded = true;
                _rb.velocity = Vector2.zero; // Stop any remaining movement
                _rb.isKinematic = true; // Stop the platform from falling further
            }
        }
    }
}