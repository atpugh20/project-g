using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    public float bounceForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collision detected");
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Bounce(bounceForce);
                Debug.Log("Bounce applied with force: " + bounceForce);
            }
        }
    }
}
