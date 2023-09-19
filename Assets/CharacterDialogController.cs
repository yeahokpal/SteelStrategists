using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogController : MonoBehaviour
{
    [SerializeField] private GameObject self;
    [SerializeField] private GameObject DialogManager;
    // Start is called before the first frame update
    void Start()
    {
        //MonoBehaviour DialogScript = GetComponent<DialogManager>();
        switch (self.name)
        {
            case ("DialogTester"):
                //DialogScript.StartDialog("Test", "This test works");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
