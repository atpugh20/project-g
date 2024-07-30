using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpawner : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject Shadow;
    public GameObject Spawner;

    private void OnTriggerEnter2D(Collider2D col) {
        Shadow.transform.position = Spawner.transform.position;
    }
}
