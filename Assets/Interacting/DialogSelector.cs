using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSelector : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject DialogCanvas;
    [SerializeField] private Sprite portrait;
    // Start is called before the first frame update
    public void SelectDialog()
    {
        Debug.Log("select dialog running");
        switch (name)
        {
            //add the portrait variable as the third parameter to add a portrait to the dialog box
            //this case shouldn't appear in actual gameplay, just for testing
            case ("[InsertCharacterName]Dialog"):
                Debug.Log("dialog tester recognized");
                DialogCanvas.GetComponent<DialogManager>().StartDialog("Test Name", "Test Dialog");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
