using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutController : MonoBehaviour {
    public GameObject VirtualCamera;
    private CinemachineVirtualCamera _vC;

    // Start is called before the first frame update
    void Start() {
        _vC = VirtualCamera.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        print("zoom out");
        _vC.ForceCameraPosition(new Vector3(_vC.transform.position.x, _vC.transform.position.y, -60), Quaternion.identity);
    }
}
