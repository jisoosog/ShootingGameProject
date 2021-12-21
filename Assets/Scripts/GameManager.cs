using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Gamemanager för menyn över spelet
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public bool isGameActive;
    public void StartGame()
    {
        Debug.Log("Game started");

        restartButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        isGameActive = true;


    }

    public void GameOver()
    {
        Debug.Log("Game over");
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }



    public void RestartGame()
    {
        Debug.Log("Game restarted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
