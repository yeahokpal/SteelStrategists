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

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogCanvas;

    private void Awake()
    {
        DialogCanvas = GameObject.Find("DialogCanvas");
    }
    public void StartDialog(string characterName, string text)//for dialog boxes with no 
    {
        if (DialogCanvas == null)
        {
            Debug.Log("Dialog Canvas Not Found");
        } 
        else
        {
            foreach (Transform child in transform) child.gameObject.SetActive(true);
            this.transform.GetChild(2).GetComponent<TMP_Text>().text = text;
            this.transform.GetChild(3).GetComponent<TMP_Text>().text = characterName;
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
            foreach (Transform child in transform) child.gameObject.SetActive(true);

            this.transform.GetChild(2).GetComponent<TMP_Text>().text = text;
            this.transform.GetChild(3).GetComponent<TMP_Text>().text = characterName;
            this.transform.GetChild(0).GetComponent<Image>().sprite = characterPortrait;
        }
    }
}
