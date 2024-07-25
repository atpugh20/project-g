using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpawner : MonoBehaviour {

    public GameObject WaterBullet;
    public WaterBulletController controller;
    private Vector3 _initialVector;
    
    // Start is called before the first frame update
    void Start() {
        controller = WaterBullet.GetComponent<WaterBulletController>();
        _initialVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            SpawnWater();
        }
    }

    void SpawnWater() {
        Vector3 SpawnPoint = gameObject.transform.position;
        //Instantiate(original:WaterBullet, position:SpawnPoint, new(0,0,0,0));
        Instantiate(WaterBullet, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
    }
}
