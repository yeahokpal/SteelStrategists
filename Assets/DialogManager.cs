using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogCanvas;

    private void Awake()
    {
        DialogCanvas = GameObject.Find("DialogCanvas");
    }
    public void StartDialog(string characterName, string text)
    {
        if (DialogCanvas == null)
        {
            Debug.Log("Dialog Canvas Not Found");
        } 
        else
        {
            Debug.Log("Character Name: " + characterName);
            Debug.Log("Dialog Text: " + text);

            foreach (Transform child in transform) child.gameObject.SetActive(true);
            
            GameObject.Find("DialogText").GetComponent<TMP_Text>().text = text;
            GameObject.Find("DialogName").GetComponent<TMP_Text>().text = characterName;

            this.transform.GetChild(2).GetComponent<TMP_Text>().text = text;
            this.transform.GetChild(3).GetComponent<TMP_Text>().text = characterName;
        }
    }
    public void StartDialog(string characterName, string text, Sprite characterPortrait)
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
