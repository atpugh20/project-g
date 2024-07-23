using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float crumbleDelay = 1f;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isOnPlatform", true);
            Invoke("Crumble", crumbleDelay);
        }
    }

    private void Crumble()
    {
        animator.Play("CrumblePlat1");
        Destroy(gameObject);
    }
}
