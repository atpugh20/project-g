using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("new checkpoint");
            FindObjectOfType<PlayerController>().Checkpoint = new Vector2(transform.position.x, transform.position.y);
        }
    }
}
