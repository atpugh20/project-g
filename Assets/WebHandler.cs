using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class WebHandler : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
    void OnCollisionEnter2D(Collision2D collision) {
        PlayerController pC = collision.gameObject.GetComponent<PlayerController>();
        if (pC.UsingNeutralFlame || pC.UsingDirectionalFlame) Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision) {
        PlayerController pC = collision.gameObject.GetComponent<PlayerController>();
        if (pC.UsingNeutralFlame || pC.UsingDirectionalFlame) Destroy(gameObject);
    }
    
}
