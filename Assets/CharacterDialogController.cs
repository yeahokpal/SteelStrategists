using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogController : MonoBehaviour
{
    [SerializeField] private GameObject self;
    [SerializeField] private GameObject DialogManager;
    // Start is called before the first frame update
    public void SelectDialog()
    {
        switch (self.name)
        {
            case ("DialogTester"):
                DialogManager.GetComponent<DialogManager>().StartDialog("Test Name", "Test Dialog");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
