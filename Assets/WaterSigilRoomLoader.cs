using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterSigilRoomLoader : MonoBehaviour {
    public int NextScene = 5;
    private void OnCollisionEnter2D(Collision2D collision) => SceneManager.LoadScene(NextScene);
}
