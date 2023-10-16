/*
 * Programmers: Jack Gill
 * Purpose: Manage systems that dont need individual scripts and that multiple classes might need to use
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Timer Related Variables
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

    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner");
    }

    void Update()
    {
        // Put in a method later
        AudioListener.volume = volumeSlider.value;

        // When the game starts...
        if (startTimer)
        {
            // Calculate the remaining time on the timer and put it into minute:second format
            timerMinutesLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) / 60;
            timerSecondsLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) % 60;
            // Update the Timer UI
            timerTxt.text = timerMinutesLeft + ":" + timerSecondsLeft;

            // When the timer is donw, start spawning enemies
            if (timerMinutesLeft == 0 && timerSecondsLeft == 0 && startSpawning)
            {
                enemySpawner.GetComponent<EnemySpawner>().StartCoroutine(enemySpawner.GetComponent<EnemySpawner>().StartSpawning());
                startSpawning = false;
            }
        }
    }
}
