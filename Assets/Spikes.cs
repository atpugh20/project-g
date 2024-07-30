using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Die(); //call death function on player controller
            }
        }
    }
}
