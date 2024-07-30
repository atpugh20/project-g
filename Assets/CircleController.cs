using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CircleController : MonoBehaviour {

    public GameObject Path;
    public float Speed = 2f;
    private Transform[] _waypoints;
    private Transform _currentPosition;

    // Start is called before the first frame update
    void Start() {
        _currentPosition = GetComponent<Transform>();
        _waypoints = Path.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _waypoints.Length; i++) {
            print(_waypoints[i].position);
        }
                
    }
    
    // Update is called once per frame
    void Update() {
        FindClosestPoint();
    }

    void FindClosestPoint() {
        Transform closest = null;
        for (int i = 1; i < _waypoints.Length; i++) {
           if (i == 1) {
                closest = _waypoints[i];
           } else {
                float oldDistance = Vector3.Distance(closest.position, _currentPosition.position);
                float newDistance = Vector3.Distance(_waypoints[i].position, _currentPosition.position);
                if (oldDistance > newDistance) closest = _waypoints[i];
            }
        }
        _currentPosition.position = Vector3.MoveTowards(_currentPosition.position, closest.position, Speed * Time.deltaTime);
        if (Vector3.Distance(closest.position, _currentPosition.position) == 0) closest.position = new Vector3(closest.position.x, closest.position.y, 10000);
        
    }

    
}
