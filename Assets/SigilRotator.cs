using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SigilRotator : MonoBehaviour {
    public GameObject SpotLight;
    private float _fakeX = 0f;
    public float WaveInterval = 0.005f;
    public GameObject getSound;
    private AudioSource _getSound;
    private AbilityController _aC;
    // Start is called before the first frame update
    void Start() {
        _aC = GameObject.Find("Player").GetComponent<AbilityController>();
        _getSound = getSound.GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update() {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 0.25f, transform.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(_fakeX) * 0.005f + transform.position.y), transform.position.z);
        SpotLight.transform.position = transform.position;
        _fakeX+=WaveInterval;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _aC.hasFlame = true;
            _getSound.Play();
            Destroy(GameObject.Find("SpotLightSigil"));
            GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
