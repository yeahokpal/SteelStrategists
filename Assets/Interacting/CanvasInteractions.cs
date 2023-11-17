using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasInteractions : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject DialogBox;
    private GameObject[] dialogObjects;
    private void Awake()
    {
        dialogObjects = GameObject.FindGameObjectsWithTag("Dialog Object");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Player Clicked Dialog Box");
        foreach (GameObject dialogItem in dialogObjects)
        {
            dialogItem.GetComponentInChildren<DialogManager>().CloseDialog();
        }
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
