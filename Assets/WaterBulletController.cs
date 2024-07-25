using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBulletController : MonoBehaviour {
    private Vector3 _initialVector;
    public float BulletSpeed;
    public float BulletMaxLife;
    private float _bulletLife = 0;
    private Vector3 newDirection;

    // Start is called before the first frame update
    void Start() {
        _initialVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
    }

    // Update is called once per frame
    void Update() {
        print(transform.position);
        newDirection = transform.position + _initialVector;
        print(newDirection);
        transform.Translate((transform.forward * BulletSpeed * Time.deltaTime));
        _bulletLife++;
        if (_bulletLife * Time.deltaTime >= BulletMaxLife) Kill();
    }

    void Kill() => Destroy(gameObject);
}
