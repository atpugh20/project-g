using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Loader : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) => SceneManager.LoadScene(5);
}
