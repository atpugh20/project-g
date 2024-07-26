using System.Collections;
using System.Collections.Generic;
using TController;
using UnityEngine;

public class WebHandler : MonoBehaviour {
    private AbilityController _aC;

    // Start is called before the first frame update
    void Start() { 
        _aC = GameObject.Find("Player").GetComponent<AbilityController>(); 
    }
    
    // Update is called once per frame
    void Update() {}

    void OnCollisionEnter2D(Collision2D collision) {
        if (_aC.UsingNeutralFlame || _aC.UsingDirectionalFlame) Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (_aC.UsingNeutralFlame || _aC.UsingDirectionalFlame) Destroy(gameObject);
    }
    
}
