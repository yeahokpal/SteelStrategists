/* Programmers: Jack Gill and Caden Mesina
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
using UnityEngine.UI;
using System.IO;
using TMPro;

public enum BotStatus { Idle, Gathering, WaitingToGather }
public enum Material { Wood, Steel, Electronics, None }

public class GameManager : MonoBehaviour
{
    #region Variables
    // Misc Variables
    [SerializeField] Camera cam;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] Texture2D cursorPoint;
    public GameObject enemySpawner;
    public int score;

    // Volume
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip battle2Song;
    [SerializeField] private AudioClip battle1Song;
    [SerializeField] private AudioClip themeSong;
    bool startPrepAudio = true;
    public float Volume = .3f;

    // Bot Variables
    public Bot[] bots = new Bot[3];
    CanvasInteractions ci;

    // Timer Variables
    private TextMeshProUGUI timerTxt;
    public float timerDelay;
    private float timerLengthSeconds = 120;
    private int timerMinutesLeft;
    private int timerSecondsLeft;
    private bool startSpawning = true;
    public bool startTimer = false;
    private bool hasStartedTimer = false;
    public bool playerHasPressedAButton = false;

    // Crash Report Variables
    public bool handleExceptions = true;
    public Log mostRecentLog = new Log();
    public static List<string> logFile = new List<string>();
    public static StreamWriter writer;
    string bug;
    #endregion

    #region Default Methods

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (writer == null)
        {
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
        SceneManager.activeSceneChanged += ChangeActiveScene;

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
                hasStartedTimer = true;
                timerDelay = Time.realtimeSinceStartup;
                startTimer = true;
                UnityEngine.Debug.Log("Timer Start");
            }
            else
            {
                timerTxt.text = "0:00";
            }

            // When the game starts...
            if (startTimer)
            {
                if (startPrepAudio && playerHasPressedAButton)
                {
                    audioSource.Play();
                    startPrepAudio = false;
                }

                // Calculate the remaining time on the timer and put it into minute:second format
                timerMinutesLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) / 60;
                timerSecondsLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) % 60;
                // Update the Timer UI

                if (timerSecondsLeft <= 9) // If the remaining seconds is <= 9, add another 0 so that "1:6" gets written as "1:06"
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
                    enemySpawner = GameObject.Find("EnemySpawner");
                    enemySpawner.GetComponent<EnemySpawner>().StartCoroutine(enemySpawner.GetComponent<EnemySpawner>().StartSpawning());
                    startSpawning = false;
                    startTimer = false;
                    audioSource.clip = battle2Song;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
        #endregion
    }
    #endregion

    #region Custom Methods

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

    public void ChangeActiveScene(Scene current, Scene next)
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        AudioSource audioSource = gm.GetComponent<AudioSource>();

        switch (next.name)
        {
            case "StartingMenu":
                audioSource.Stop();
                audioSource.clip = themeSong;
                audioSource.Play();
                break;
            case "MainScene":
                audioSource.Stop();
                audioSource.clip = battle1Song;
                break;
            case "Tutorial":
                audioSource.Stop();
                audioSource.clip = battle2Song;
                audioSource.Play();
                break;
        }

        // Put here because it is called on scene changes
        score = 0;
        timerDelay = Time.realtimeSinceStartup;
        timerSecondsLeft = 0;
        timerMinutesLeft = 0;
        hasStartedTimer = false;
        startSpawning = true;
        startTimer = true;
        playerHasPressedAButton = false;
        bots[0] = new Bot();
        bots[1] = new Bot();
        bots[2] = new Bot();

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

        ChangeVolume(Volume);
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
        if (bots[0] == null) bots[0] = new Bot();
        if (bots[1] == null) bots[1] = new Bot();
        if (bots[2] == null) bots[2] = new Bot();
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
    #endregion
}
