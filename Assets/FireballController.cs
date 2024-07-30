using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {

    private Rigidbody2D _rb;
    private AbilityController _aC;
    private Animator _anim;
    public int speed = 5;
    public int MaxLife = 5;
    private float _life = 0;

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _aC = GameObject.Find("Player").GetComponent<AbilityController>();
        _anim = GetComponent<Animator>();
        _anim.SetTrigger("bigFlame");
    }

    // Update is called once per frame
    void Update(){
        _life+=Time.deltaTime;
        if (_life >= MaxLife) StartKill();
        _rb.velocity = _aC.NeutralMoveDir * speed;
    }

    public void StartKill() => Destroy(gameObject);
}
