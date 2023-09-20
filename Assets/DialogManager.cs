using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogCanvas;
    public void StartDialog(string characterName, string text)
    {
        Debug.Log("Character Name: " + characterName);
        Debug.Log("Dialog Text: " + text);

        DialogCanvas.SetActive(true);
        GameObject.Find("DialogText").GetComponent<TextMesh>().text = text;
        GameObject.Find("DialogName").GetComponent<TextMesh>().text = characterName;

        /*DialogCanvas.transform.GetChild(2).GetComponent<TextMesh>().text = text;
        DialogCanvas.transform.GetChild(3).GetComponent<TextMesh>().text = characterName;
        DialogCanvas.SetActive(true);*/
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
