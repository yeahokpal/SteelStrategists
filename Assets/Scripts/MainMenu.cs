/*
 * Programmer: Dalton Bartholomew
 * Purpose: Control Main Menu Buttons
 * Input: Player Presses buttons
 * Output: Change settings and open scenes and quit games
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] Slider slider;

    // Start Button
    public void StartGame ()
    {
        // goes to the main play scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    // Quit Button
    public void QuitGame ()
    {
        // Quits the scene 
        Debug.Log("QUITE!!!!!");
        Application.Quit();
    }

    // Change the volume
    public void ChangeVolume()
    {
        float volume = slider.value;
        gm.ChangeVolume(volume);
    }
}
