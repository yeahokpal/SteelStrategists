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
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Web;

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

    #region Crash Report Variables

    public bool handleExceptions = true;
    public Log mostRecentLog = new Log();
    string bug;

    #endregion
    private void Awake()
    {
        // Calls Method whenever something is logged to the console (for crash reporting)
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

    #region Crash Reporting

    public class Log
    {
        public string logString{get; set;}
        public string stackTrace{get; set;}
    }

    void HandleException(string logString, string stackTrace, LogType type)
    {
        // If the log is an error 
        if (type == LogType.Exception && handleExceptions)
        {
            mostRecentLog.logString = logString;
            mostRecentLog.stackTrace = stackTrace;

            // the string we use for crashinfo
            bug = "An exception has occurred!\nLocation:\n" + mostRecentLog.stackTrace + "Issue:\n" + mostRecentLog.logString;
            AddLog(bug);

            // This line just helps to differentiate Exceptions
            AddLog("--------------------------");
            DumpLogs(); // Dump new logs
            
            // PLEASE REMEMBER TO UNCOMMENT THIS LINE OH MY GOD!!!!!
            //SendBugReport(bug);
        }
    }

    public static List<string> logFile = new List<string>();

    public static void AddLog(string logString)
    {
        logFile.Add(logString);
    }

    public static void DumpLogs()
    {
        // If the Debug folder doesn't exist, create it
        if (!Directory.Exists(Application.dataPath + "/Debug/"))
        {
            AddLog("Debug directory did not exist");
            Directory.CreateDirectory(Application.dataPath + "/Debug/");
        }

        StreamWriter writer = new StreamWriter(Application.dataPath + "/Debug/log.txt", true);

        foreach (string logString in logFile)
        {
            writer.WriteLine(logString);
        }

        writer.Close();
    }

    // Sending email of the bug report
    public void SendBugReport(string bug)
    {
        // Inform player that an error has occurred
        Process.Start(Application.dataPath + "/Debug/CrashDialogBox.exe");

        //Open Mailto link
        System.Diagnostics.Process.Start("mailto:ps24jgill@efcts.us?subject=Steel%20Strategists%20Report&" +
            "body=Hello,%20I%20have%20experienced%20a%20crash%20while%20playing%20Steel%20Strategists.%20Here%20is%20my%20crash%20report:%0A" + bug.Replace("\n", "%0A"));

        Application.Quit();
    }

    #endregion
}
