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

    public bool handleExceptions = true;
    public Log mostRecentLog = new Log();
    string bug;

    #endregion
    private void Awake()
    {
        // Calls Method whenever something is logged to the console
        Application.logMessageReceived += HandleException;
    }
    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner");

        // Me purposefully causing errors to cause Exceptions
        int[] test = new int[1];
        int haha = test[2]; // Out of array bounds
    }
    void Update()
    {
        AudioListener.volume = volumeSlider.value; // Put in a method later

        #region Timer Stuff

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
    }

    public class Log
    {
        public string logString{get; set;}
        public string stackTrace{get; set; }
    }


    void HandleException(string logString, string stackTrace, LogType type)
    {
        // If the log is an error 
        if (type == LogType.Exception && handleExceptions)
        {
            LogToFile.Log("Crash at " + Time.realtimeSinceStartup.ToString());

            mostRecentLog.logString = logString;
            mostRecentLog.stackTrace = stackTrace;

            // the string we use for crashinfo
            bug = "An exception has occured!\nLocation:\n" + mostRecentLog.stackTrace + "Issue:\n" + mostRecentLog.logString;
            LogToFile.Log(bug);

            // This line just helps to differentiate Exceptions
            LogToFile.Log("--------------------------");
            LogToFile.DumpLogs(); // Dump new log
        }
    }

    public void UserDump()
    {
        LogToFile.Log("Dump at " + Time.realtimeSinceStartup.ToString());
        LogToFile.DumpLogs();
    }

    public void SendBugReport()
    {
        // the string we use for the actual email body
        string emailBug = "#### An exception has occured! ####\n\n\n#### Location ####\n" + mostRecentLog.stackTrace + "\n\n#### Issue ####\n" + mostRecentLog.logString;

        // should be sent like:
        /*
         * #### an exception has occurred! ####
         * 
         * 
         * #### location ####
         * assets/scripts/badFile.cs
         * 
         * 
         * #### issue ####
         * stack overflow something something
         * 
         * 
         * #### user input ####
         * i was just "jaunting with the boys" officer
         */

        Email.SendEmail(emailBug + "\n\n\n#### User Input ####\n" + bug);
    }
}
