/*
 * Programmers: Caden Mesina
 * Purpose: Closes dialog boxes when they're clicked
 * Input: Player clicks on canvas
 * Output: N/A
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasInteractions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject DialogBox;
    private GameObject[] dialogObjects;
    private void Awake()
    {
        dialogObjects = GameObject.FindGameObjectsWithTag("Dialog Object");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Player Clicked Dialog Box");
        foreach (GameObject dialogItem in dialogObjects)
        {
            dialogItem.GetComponentInChildren<CanvasManager>().CloseCanvas();
        }
    }

    public void SelectButtonClicked()
    {
        Debug.Log("Select Button Clicked");
    }
}
