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
    bool startTimer = true;
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Put in a method later
        AudioListener.volume = volumeSlider.value;

        if (startTimer)
        {
            timerTxt.text = (int)(timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) / 60 + ":" + (timerLengthSeconds - (int)Time.realtimeSinceStartup + timerDelay) % 60;
        }
    }
}
