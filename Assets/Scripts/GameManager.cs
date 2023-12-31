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
    private float timerLengthSeconds = 120;
    private int timerMinutesLeft;
    private int timerSecondsLeft;
    private bool startSpawning = true;
    public bool startTimer = false;
    private bool hasStartedTimer = false;
    public bool playerHasPressedAButton = false;
    TextMeshProUGUI timerTxt;

    // Crash Report Variables
    public bool handleExceptions = true;
    public Log mostRecentLog = new Log();
    public static List<string> logFile = new List<string>();
    public static StreamWriter writer;
    string bug;

    private void Awake()
    {
        if (GameObject.Find("GameManager") == this.gameObject)
        {
            DontDestroyOnLoad(gameObject);
            writer = new StreamWriter(Application.dataPath + "/ErrorLog/log.txt", true);
        }

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
    }

    void Start()
    {
        // Creating the database file and directory
        if (File.Exists(Application.dataPath + "/ErrorLog/"))
        {
            Directory.Delete(Application.dataPath + "/ErrorLog/");
        }
        Directory.CreateDirectory(Application.dataPath + "/ErrorLog/");

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
            if (playerHasPressedAButton && !hasStartedTimer)
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
        score = 0;

        // Put here because it is called on scene changes
        if (GameObject.Find("StartBotButton"))
        {
            ci = GameObject.Find("StartBotButton").GetComponent<CanvasInteractions>();
        }

        // Put here because it is called on scene changes
        if (GameObject.Find("StartBotButton"))
        {
            ci = GameObject.Find("StartBotButton").GetComponent<CanvasInteractions>();
        }

        // Put here because it is called on scene changes
        if (GameObject.Find("EnemySpawner"))
        {
            enemySpawner = GameObject.Find("EnemySpawner");

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

            SendBugReport(bug);
        }
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
        UnityEngine.Debug.Log("Updating bot...");
        bots[botNum - 1].currentStatus = BotStatus.Gathering;
        switch (tileType)
        {
            // Grass gives wood
            case TileType.Grass:
                bots[botNum - 1].currentMaterial = Material.Wood;
                break;
            // Rock gives steel
            case TileType.Rock:
                bots[botNum - 1].currentMaterial = Material.Steel;
                break;
            // Water gives wood or steel
            case TileType.Water:
                System.Random rand = new System.Random();
                if (rand.Next() > .5f) { bots[botNum - 1].currentMaterial = Material.Wood; }
                else { bots[botNum - 1].currentMaterial = Material.Steel; }
                bots[botNum - 1].currentMaterial = Material.Wood;
                break;
            // Desert gives electronics
            case TileType.Desert:
                bots[botNum - 1].currentMaterial = Material.Electronics;
                break;
        }

        UnityEngine.Debug.Log("Bot #" + (botNum) + " | Status: " + bots[botNum - 1].currentStatus.ToString());
        StartCoroutine(GatherMaterial(botNum));

    }

    IEnumerator GatherMaterial(int botNum)
    {
        GameObject currentBot = GameObject.Find("Bot" + botNum);

        // This line will give an error when not in MainScene, don't worry it will still work hopefully
        // Updates the physical sprite in the game world
        currentBot.GetComponent<SpriteRenderer>().enabled = false;

        // It takes 30 seconds for a bot to gather materials
        yield return new WaitForSeconds(30f);

        currentBot.GetComponent<SpriteRenderer>().enabled = true;

        // Updating bot variables
        bots[botNum - 1].currentStatus = BotStatus.WaitingToGather;
        currentBot.GetComponent<Interactables>().heldMaterial = bots[botNum - 1].currentMaterial;

        // Finding which sprite to have the bot be holding
        SpriteRenderer childSprite = currentBot.GetComponent<Interactables>().child.GetComponent<SpriteRenderer>();

        switch (bots[botNum - 1].currentMaterial)
        {
            case Material.Wood:
                childSprite.sprite = currentBot.GetComponent<Interactables>().materialSprites[0];
                break;
            case Material.Steel:
                childSprite.sprite = currentBot.GetComponent<Interactables>().materialSprites[1];
                break;
            case Material.Electronics:
                childSprite.sprite = currentBot.GetComponent<Interactables>().materialSprites[2];
                break;
        }

        currentBot.GetComponent<Interactables>().UpdateBot();
    }

    #endregion
}
