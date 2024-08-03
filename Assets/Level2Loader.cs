using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Loader : MonoBehaviour {
    public int NextScene = 4;
    private void OnCollisionEnter2D(Collision2D collision) => SceneManager.LoadScene(NextScene);
}
