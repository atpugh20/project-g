using System.Collections;
using System.Collections.Generic;
using TController;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowController : MonoBehaviour {

    public GameObject Player;
    public GameObject Path;

    private Rigidbody2D _rb;
    private Transform[] _waypoints;

    private int _nwp = 1; // next waypoint
    public float Speed = 20;

    private float _distToPlayer;
    private float _distToWaypoint;
    private Vector3 _target;

    private bool chasing = true;
    private int frame = 0;
    private float time = 5f;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _waypoints = Path.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update() {
        time+=Time.deltaTime;
        if (time > 3) chasing = true;
        
        if (chasing) {
            _distToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            float _angle = Vector3.SignedAngle(transform.position, Player.transform.position, new(0,0,0));
            print(_angle);
            
            _distToWaypoint = Vector3.Distance(transform.position, _waypoints[_nwp].position);
            if (_distToPlayer < _distToWaypoint) {
                _target = Player.transform.position;
            } else {
                _target = _waypoints[_nwp].position;
                if (transform.position == _target) _nwp++;
                if (_nwp == _waypoints.Length) Destroy(gameObject);
            }
            Vector3 newPos = Vector3.MoveTowards(transform.position, _target, Speed * Time.deltaTime);
            float newAngle = transform.position.x - newPos.x;
            if (newAngle < 0) {
                transform.eulerAngles = new Vector3(0, 0, 0);
            } else {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            transform.position = newPos;
        } else {
            //reverse animation
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null) {
                playerController.Die(); //call death function on player controller
                chasing = false;
            }
        }
        if (collision.CompareTag("FlameBall")) {
            chasing = false;
            time = 0f;
        }
    }
}
