using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDestroyer : MonoBehaviour {
    GameObject WaterBullet;
    void Start() => WaterBullet = transform.parent.gameObject;
   
    void Kill() => Destroy(WaterBullet);

}
