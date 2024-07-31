using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public void Home()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

}
