/*
 * Programmers: Caden Mesina
 * Purpose: Take input from DialogSelector.cs to display the dialog box on screen
 * Input: Nothing from players, StartDialog is just called by DialogSelector.cs and is given a name and dialog text and maybe a portrait
 * Output: Displays dialog box
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogCanvas;
    [SerializeField] private GameObject Player;
    [SerializeField] private Sprite Portrait;

    private void Awake()
    {
        DialogCanvas = GameObject.Find("DialogCanvas");
        Player = GameObject.Find("Player");
    }
    public void SelectDialog()
    {
        switch (name)
        {
            //add the portrait variable as the third parameter to add a portrait to the dialog box
            //this case shouldn't appear in actual gameplay, just for testing
            case ("[InsertCharacterName]Dialog"):
                StartDialog("Test Name", "Test Dialog");
                break;
        }
    }
    public void StartDialog(string characterName, string text)//for dialog boxes with no 
    {
        if (DialogCanvas == null)
        {
            Debug.Log("Dialog Canvas Not Found");
        }
        else
        {
            foreach (Transform child in DialogCanvas.transform) child.gameObject.SetActive(true);
            DialogCanvas.transform.GetChild(2).GetComponent<TMP_Text>().text = text;
            DialogCanvas.transform.GetChild(3).GetComponent<TMP_Text>().text = characterName;
            if (Player == null) Debug.Log("player not found");
            else
            {
                Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                Debug.Log("Switching map");
            }
        }
    }
    public void StartDialog(string characterName, string text, Sprite characterPortrait)
    {
        if (DialogCanvas == null)
        {
            Debug.Log("Dialog Canvas Not Found");
        }
        else
        {
            foreach (Transform child in DialogCanvas.transform) child.gameObject.SetActive(true);

            transform.GetChild(2).GetComponent<TMP_Text>().text = text;
            transform.GetChild(3).GetComponent<TMP_Text>().text = characterName;
            transform.GetChild(0).GetComponent<Image>().sprite = characterPortrait;
        }
    }
    public void CloseDialog()
    {
        foreach (Transform child in DialogCanvas.transform) child.gameObject.SetActive(false);
        Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }
}
