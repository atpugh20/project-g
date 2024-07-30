using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _waypoints = Path.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update() {
        _distToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        _distToWaypoint = Vector3.Distance(transform.position, _waypoints[_nwp].position);
        if (_distToPlayer < _distToWaypoint) { 
            _target = Player.transform.position; 
        } else {
            _target = _waypoints[_nwp].position;
            if (transform.position == _target) _nwp++;
            if (_nwp == _waypoints.Length) Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, _target, Speed * Time.deltaTime); 
    }
}
