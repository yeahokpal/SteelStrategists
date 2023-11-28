using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame ()
    {
        // goes to the main play scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame ()
    {
        // Quits the scene 
        Debug.Log("QUITE!!!!!");
        Application.Quit();
    }
}
