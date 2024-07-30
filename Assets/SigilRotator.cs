using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SigilRotator : MonoBehaviour {
    public GameObject SpotLight;
    private float _fakeX = 0f;
    public float WaveInterval = 0.005f;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 0.25f, transform.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(_fakeX) * 0.005f + transform.position.y), transform.position.z);
        SpotLight.transform.position = transform.position;
        _fakeX+=WaveInterval;
    }
}
