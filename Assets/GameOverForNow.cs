using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverForNow : MonoBehaviour
{

    [SerializeField] GameObject gameOver;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameOverScreen();
        }
    }


    public void gameOverScreen()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }



}
