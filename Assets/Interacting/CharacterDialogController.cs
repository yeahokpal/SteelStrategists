using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogController : MonoBehaviour
{ 
    [SerializeField] private GameObject DialogCanvas;
    // Start is called before the first frame update
    public void SelectDialog()
    {
        switch (this.name)
        {
            case ("DialogTester"):
                DialogCanvas.GetComponent<DialogManager>().StartDialog("Test Name", "Test Dialog");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}