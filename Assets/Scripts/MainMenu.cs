/* Programmer: Dalton Bartholomew
 * Purpose: Control Main Menu Buttons
 * Input: Player Presses buttons
 * Output: Change settings and open scenes and quit games
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class MainMenu : MonoBehaviour
{
    #region Global Variables
    private GameManager gm;
    [SerializeField] private SaveManager sm;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI[] scoretxts;
    #endregion

    #region Default Methods
    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Reading the high scores and filling them into the text boxes
        List<string> scores = sm.ReadScores();
        scores.Sort();
        scores.Reverse();
        for (int i = 0; i < 10; i++)
        {
            string[] text = new string[2];
            text = scores[i].Split(',');
            if (scores[i] != "0,___")
            {
                scoretxts[i].text = text[1] + " - " + text[0];
            }
        }
    }
    #endregion

    #region Custom Methods
    // Load Main Level
    public void StartGame ()
    {
        // goes to the main play scenes
        SceneManager.LoadScene("MainScene");
    }
    
    // Quit Application
    public void QuitGame ()
    {
        // Quits the scene 
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    // Load Tutorial Stage
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Options()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Slider"));
    }

    public void BackFromOptions()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Options Button"));
    }

    // Change the volume
    public void ChangeVolume()
    {
        float volume = slider.value;
        gm.ChangeVolume(volume);
    }
    #endregion

}
