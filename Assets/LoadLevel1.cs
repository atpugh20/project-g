using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel1 : MonoBehaviour {
    public int TargetScene = 3;
    private void OnCollisionEnter2D(Collision2D collision) => SceneManager.LoadScene(TargetScene);
}
