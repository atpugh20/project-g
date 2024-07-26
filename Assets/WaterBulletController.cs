using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBulletController : MonoBehaviour {
    Vector2 Move;
    float InitialVector;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        print(transform.right);
        _rb.velocity = transform.right;
        print(_rb.velocity);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
