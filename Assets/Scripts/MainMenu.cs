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
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] SaveManager sm;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI[] scoretxts; 

    private void Start()
    {
        List<string> scores = sm.ReadScores();
        for (int i = 0; i < 10; i++)
        {
            string[] text = new string[2];
            text = scores[i].Split(',');
            scoretxts[i].text = text[1] + " - " + text[0];
        }
        scores.Sort();
    }

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
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    // Change the volume
    public void ChangeVolume()
    {
        float volume = slider.value;
        gm.ChangeVolume(volume);
    }
}
