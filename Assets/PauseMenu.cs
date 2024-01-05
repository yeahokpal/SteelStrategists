using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    // Dalton did this 
    //Make sure the game pauses 
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
        // check if you want to pause the game 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Resume();
        }
        else {
            Pause();
        }
    }
    public void Resume()
    {
        //makes everything back to normal
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        // brings the pause menu stops time
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartingMenu");
    }
    public void QuitMenu()
    {
        Debug.Log("Quiting game");
        Application.Quit();
    }
}

