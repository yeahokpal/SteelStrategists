/* Programmers: Jack Gill
 * This script is to manage individual game elements that arent enough for their own scripts
 * Purposes: - Manage Game Timer
 *           - Catch Exceptions and Send Emails
 *           - Control the score of the current game
 */

using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    // Volume
    public float Volume = 1f;

    // Bot Variables
    public Bot[] bots = new Bot[3];
    CanvasInteractions ci;

    // Timer Variables
    public float timerDelay;
    float timerLengthSeconds = 60;
    int timerMinutesLeft;
    int timerSecondsLeft;
    bool startSpawning = true;
    public bool startTimer = false;
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
        // Initializing Bots
        bots[0] = new Bot();
        bots[1] = new Bot();
        bots[2] = new Bot();

        if (GameObject.Find("StartBotButton"))
        {
            ci = GameObject.Find("StartBotButton").GetComponent<CanvasInteractions>();
        }

        Cursor.SetCursor(cursorPoint, Vector2.zero, CursorMode.ForceSoftware);
        SceneManager.activeSceneChanged += ChangeVolume;

        // Calls HandleException Method whenever something is logged to the console (for crash reporting)
        Application.logMessageReceived += HandleException;

        // For closing the writer when the player quits
        Application.quitting += Quit;

        if (!GameObject.Find("GameManager"))
        {
            DontDestroyOnLoad(this);
        }

    }

    // For closing the writer when the player quits
    private void Quit()
    {
        writer.Close();
    }

    void Start()
    {
        // Creating the database file and directory
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
        #region Timer Stuff
        
        UpdateScoreTxt();

        // If the timer exists in the current scene, when do the timer stuff
        if (GameObject.Find("txtTimer") && timerTxt == null)
        {
            timerTxt = GameObject.Find("txtTimer").GetComponent<TextMeshProUGUI>();
        }

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

    #region Volume changing
    // - Used to update the volume whenever the slider value changes
    public void ChangeVolume(float newVolume)
    {
        Volume = newVolume;
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects){
            if (go.GetComponent<AudioSource>())
            {
                go.GetComponent<AudioSource>().volume = newVolume;
            }
        }
    }

    // - Used to update the volume of each GameObject whenever the scene changes
    public void ChangeVolume(Scene current, Scene next)
    {
        // Put here because it is called on scene changes
        if (GameObject.Find("StartBotButton"))
        {
            ci = GameObject.Find("StartBotButton").GetComponent<CanvasInteractions>();
        }

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<AudioSource>())
            {
                go.GetComponent<AudioSource>().volume = Volume;
            }
        }
    }
    #endregion

    #region Scores
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
    #endregion

    #region Crash Reporting

    public class Log
    {
        public string logString{get; set;}
        public string stackTrace{get; set;}
    }

    // Gets called whenever something is logged to console, error or intentional
    void HandleException(string logString, string stackTrace, LogType type)
    {
        /*
        // Handling generic logs
        if (type == LogType.Log)
        {
            writer.WriteLine(logString);
        }
        // Handling Errors
        else if (type == LogType.Exception && handleExceptions)
        {
            mostRecentLog.logString = logString;
            mostRecentLog.stackTrace = stackTrace;

            // the string we use for crashinfo
            bug = "An exception has occurred!\nLocation:\n" + mostRecentLog.stackTrace + "Issue:\n" + mostRecentLog.logString;
            logFile.Add(bug);

            // This line just helps to differentiate Exceptions
            logFile.Add("--------------------------");
            DumpLogs(); // Dump new logs

            // PLEASE REMEMBER TO UNCOMMENT THIS LINE OH MY GOD!!!!!
            //SendBugReport(bug);
        }*/
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

    #region Bot Handling

    // Called whenever a bot is sent to do a task
    public void UpdateBot(int botNum, TileType tileType)
    {
        botNum--;
        switch (tileType)
        {
            // Grass gives wood
            case TileType.Grass:
                bots[botNum].currentMaterial = Material.Wood;
                break;
            // Rock gives steel
            case TileType.Rock:
                bots[botNum].currentMaterial = Material.Steel;
                break;
            // Water gives wood or steel
            case TileType.Water:
                System.Random rand = new System.Random();
                if (rand.Next() > .5f) { bots[botNum].currentMaterial = Material.Wood; }
                else { bots[botNum].currentMaterial = Material.Steel; }
                bots[botNum].currentMaterial = Material.Wood;
                break;
            // Desert gives electronics
            case TileType.Desert:
                bots[botNum].currentMaterial = Material.Electronics;
                break;
        }
        bots[botNum].currentStatus = BotStatus.Gathering;

        UnityEngine.Debug.Log("Bot #" + (botNum + 1) + " | Status: " + bots[botNum].currentStatus.ToString());
        StartCoroutine(GatherMaterial(botNum));

    }

    IEnumerator GatherMaterial(int botNum)
    {
        // This line will give an error when not in MainScene, don't worry it will still work hopefully
        // Updates the physical sprite in the game world
        GameObject.Find("Bot" + botNum).GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(30f);

        GameObject.Find("Bot" + botNum).GetComponent<SpriteRenderer>().enabled = true;

        bots[botNum].currentStatus = BotStatus.WaitingToGather;

        switch(bots[botNum].currentMaterial) 
        {
            case Material.Wood:
                break;



        }
    }

    #endregion
}
