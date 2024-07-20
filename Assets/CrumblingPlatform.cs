using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float crumbleDelay = 1f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Crumble", crumbleDelay);
        }
    }

    private void Crumble()
    {
        Destroy(gameObject);
    }
}
