using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //(Brackeys, 2017)

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
void Pause()
{
    Debug.Log("Pause menu activated");
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
}

public void Resume()
{
    Debug.Log("Resuming game");
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;
}
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu"); // Call the Main Menu
        }

public void ExitGame()
    {
        Application.Quit(); // Exit the application
    }




    /* References
     * Authors: Brackeys
     * Source: Youtube
     * Date: Dec 20, 2017
     * URL: https://www.youtube.com/watch?v=JivuXdrIHK0&ab_channel=Brackeys
     */
}
