using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnableCrumblePlat : MonoBehaviour
{
    public float crumbleDelay = 1f;
    public float respawnDelay = 5f; // Time before the platform respawns
    public string respawnAnimationName = "RespawnNewCrumblePlat";
    public string idleAnimationName = "Idle";

    private Animator animator;
    private Vector3 originalPosition;
    private Collider2D platformCollider;
    private SpriteRenderer platformRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected on platform. Starting crumble sequence.");
            animator.SetBool("isOnPlatform", true);
            Invoke("Crumble", crumbleDelay);
        }
    }

    private void Crumble()
    {
        Debug.Log("Platform crumbling.");
        animator.Play("CrumblePlat1");
        platformCollider.enabled = false;
        platformRenderer.enabled = false;
        Invoke("Respawn", respawnDelay);
    }

    private void Respawn()
    {
        Debug.Log("Respawning platform.");
        transform.position = originalPosition;
        platformCollider.enabled = true;
        platformRenderer.enabled = true;
        animator.SetBool("isOnPlatform", false);
        StartCoroutine(PlayRespawnAnimation());
    }

    private IEnumerator PlayRespawnAnimation()
    {
        animator.Play("RespawnNewCrumblePlat");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Play("CrumbleIdle");
    }
}