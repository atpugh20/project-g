using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBulletController : MonoBehaviour {
    
    private Rigidbody2D _rb;
    private Transform _transform;
    private Animator _anim;
    private Vector2 _move;
    private bool _col = false;
    public int Speed = 30;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _anim = GetComponentInChildren<Animator>();
        _move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _anim.SetBool("isMoving", true);
        float newAngle = Mathf.Atan2(_move.y, _move.x) * Mathf.Rad2Deg;
        _transform.eulerAngles = new Vector3(0f, _transform.eulerAngles.y, newAngle);
    }

    // Update is called once per frame
    void Update() {
        if (!_col) _rb.velocity = _move * Speed;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) return;
        _col = true;
        _rb.velocity = Vector2.zero;
        _anim.SetBool("isMoving", false);
    }


    public void Kill() => Destroy(gameObject);
}
