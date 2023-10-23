/*
 * Programmers: Jack Gill
 * 
 * This script is to manage individual game elements that arent enough for their own scripts
 * 
 * Purposes: - Manage Game Timer
 *           - Catch Exceptions and Send Emails
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Timer Variables

    float timerDelay;
    float timerLengthSeconds = 120;
    int timerMinutesLeft;
    int timerSecondsLeft;
    bool startSpawning = true;
    bool startTimer = true;
    // Timer UI Elements
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] Slider volumeSlider;
    public GameObject enemySpawner;

    #endregion

    #region CrashReport Variables
    bool hasNewLog;

    #endregion

    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner");
    }

    void Update()
    {
        #region Timer Stuff

        // Put in a method later
        AudioListener.volume = volumeSlider.value;

        // When the game starts...
        if (startTimer)
        {
            // Calculate the remaining time on the timer and put it into minute:second format
            timerMinutesLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) / 60;
            timerSecondsLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) % 60;
            // Update the Timer UI

            if (timerSecondsLeft <= 9) // If the remaining seconds is <= 9, add another 0 so that "1:6" is actually "1:06"
            {
                timerTxt.text = timerMinutesLeft + ":0" + timerSecondsLeft;
            }
            else
            {
                timerTxt.text = timerMinutesLeft + ":" + timerSecondsLeft;
            }

            // When the timer is donw, start spawning enemies
            if (timerMinutesLeft == 0 && timerSecondsLeft == 0 && startSpawning)
            {
                enemySpawner.GetComponent<EnemySpawner>().StartCoroutine(enemySpawner.GetComponent<EnemySpawner>().StartSpawning());
                startSpawning = false;
            }
        }
        #endregion

        Application.logMessageReceived += HandleException;

    }
    void HandleException(string logString, string stackTrace, LogType type)
    {

    }
}
