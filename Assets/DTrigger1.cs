using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class DTrigger1 : MonoBehaviour
{

    public GameObject player;
    private PlayerController PlayerController;
    public GameObject dialoguebox;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController = player.GetComponent<PlayerController>();
        animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //PlayerController.Grab();
            //animator.SetTrigger("isGrabbing");
            //print(dialoguebox);
            //StartCoroutine(dboxappear());
            dboxappear1();
        }
    }


    private void dboxappear1()
    {
        dialoguebox.SetActive(true);
    }


}
