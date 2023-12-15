/* Programmers: Jack Gill
 * This script is to manage individual game elements that arent enough for their own scripts
 * Purposes: - Manage Game Timer
 *           - Catch Exceptions and Send Emails
 *           - Control the score of the current game
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Web;

public class GameManager : MonoBehaviour
{
    // Misc Variables
    [SerializeField] Camera cam;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI scoreTxt;
    public GameObject enemySpawner;
    [SerializeField] Texture2D cursorPoint;
    public int score;

    // Bot Variables
    public Bot[] bots = new Bot[3];

    // Timer Variables
    float timerDelay;
    float timerLengthSeconds = 120;
    int timerMinutesLeft;
    int timerSecondsLeft;
    bool startSpawning = true;
    bool startTimer = false;
    bool hasStartedTimer = false;
    TextMeshProUGUI timerTxt;

    // Crash Report Variables
    public bool handleExceptions = true;
    public Log mostRecentLog = new Log();
    public static List<string> logFile = new List<string>();
    public static StreamWriter writer = new StreamWriter(Application.dataPath + "/ErrorLog/log.txt", true);
    string bug;

    private void Awake()
    {
        Cursor.SetCursor(cursorPoint, Vector2.zero, CursorMode.ForceSoftware);

        // Calls HandleException Method whenever something is logged to the console (for crash reporting)
        Application.logMessageReceived += HandleException;

        // For closing the writer when the player quits
        Application.quitting += Quit;

        DontDestroyOnLoad(this);
    }

    // For closing the writer when the player quits
    private void Quit()
    {
        writer.Close();
    }

    void Start()
    {
        if (File.Exists(Application.dataPath + "/ErrorLog/"))
        {
            Directory.Delete(Application.dataPath + "/ErrorLog/");
        }
        Directory.CreateDirectory(Application.dataPath + "/ErrorLog/");

        enemySpawner = GameObject.Find("EnemySpawner");

        // Me purposefully causing errors to cause Exceptions
        //int[] test = new int[1];
        //int haha = test[2]; // array out of bounds
    }
    void Update()
    {
        UpdateScoreTxt();

        // If the timer exists in the current scene, when do the timer stuff
        if (GameObject.Find("txtTimer") && timerTxt == null)
        {
            timerTxt = GameObject.Find("txtTimer").GetComponent<TextMeshProUGUI>();
        }

        #region Timer Stuff

        if (timerTxt != null)
        {
            // Adds timer delay for whenever the player presses a button
            if (Input.anyKey && !hasStartedTimer)
            {
                timerDelay = Time.realtimeSinceStartup;
                hasStartedTimer = true;
                startTimer = true;
            }


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
                if (timerMinutesLeft <= 0 && timerSecondsLeft <= 0 && startSpawning)
                {
                    enemySpawner.GetComponent<EnemySpawner>().StartCoroutine(enemySpawner.GetComponent<EnemySpawner>().StartSpawning());
                    startSpawning = false;
                    startTimer = false;
                }
            }
        }
        #endregion

    }

    // Volume changing
    public void ChangeVolume(float volume)
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects){
            if (go.GetComponent<AudioSource>())
            {
                go.GetComponent<AudioSource>().volume = volume;
            }
        }
    }

    // Updating Score Text UI
    public void UpdateScoreTxt()
    {
        if (GameObject.Find("txtScore"))
        {
            if (scoreTxt == null)
            {
                scoreTxt = GameObject.Find("txtScore").GetComponent<TextMeshProUGUI>();
            }
            scoreTxt.text = string.Format("{0:0000000000}", score);
        }
    }

    #region Crash Reporting

    public class Log
    {
        public string logString{get; set;}
        public string stackTrace{get; set;}
    }

    // Gets called whenever something is logged to console, error or intentional
    void HandleException(string logString, string stackTrace, LogType type)
    {
        //// Handling generic logs
        //if (type == LogType.Log)
        //{
        //    writer.WriteLine(logString);
        //}
        //// Handling Errors
        //else if (type == LogType.Exception && handleExceptions)
        //{
        //    mostRecentLog.logString = logString;
        //    mostRecentLog.stackTrace = stackTrace;

        //    // the string we use for crashinfo
        //    bug = "An exception has occurred!\nLocation:\n" + mostRecentLog.stackTrace + "Issue:\n" + mostRecentLog.logString;
        //    logFile.Add(bug);

        //    // This line just helps to differentiate Exceptions
        //    logFile.Add("--------------------------");
        //    DumpLogs(); // Dump new logs
            
        //    // PLEASE REMEMBER TO UNCOMMENT THIS LINE OH MY GOD!!!!!
        //    //SendBugReport(bug);
        //}
    }

    // Write all logs to log.txt if there is an error
    public static void DumpLogs()
    {
        foreach (string logString in logFile)
        {
            writer.WriteLine(logString);
        }

        // For closing the writer when the game crashes, because this method only gets called on crash
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
