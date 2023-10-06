using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    float timerDelay;
    float timerLengthSeconds = 120;
    int timerMinutesLeft;
    int timerSecondsLeft;
    bool startSpawning = true;

    bool startTimer = true;
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] Slider volumeSlider;
    public GameObject enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner");
    }

    // Update is called once per frame
    void Update()
    {
        // Put in a method later
        AudioListener.volume = volumeSlider.value;

        // When the game starts...
        if (startTimer)
        {
            timerMinutesLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) / 60;
            timerSecondsLeft = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) % 60;
            // Update the Timer UI
            timerTxt.text = timerMinutesLeft + ":" + timerSecondsLeft;

            if (timerMinutesLeft == 0 && timerSecondsLeft == 0 && startSpawning)
            {
                enemySpawner.GetComponent<EnemySpawner>().StartCoroutine(enemySpawner.GetComponent<EnemySpawner>().StartSpawning());
                startSpawning = false;
            }
        }
    }
}
