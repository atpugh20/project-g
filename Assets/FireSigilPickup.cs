using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSigilPickup : MonoBehaviour
{

    public GameObject player;
    private AbilityController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = player.GetComponent<AbilityController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            controller.CanUseDirFlame = true;
        }
    }

}
