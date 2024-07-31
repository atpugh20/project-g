using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireSigilRoomLoader : MonoBehaviour {
    public int TargetScene = 2;
    private void OnCollisionEnter2D(Collision2D collision) => SceneManager.LoadScene(TargetScene);
}
