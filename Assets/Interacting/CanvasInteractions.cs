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
using UnityEngine.InputSystem;

public class CanvasInteractions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject MapScreen;
    [SerializeField] private GameObject Player;
    private GameObject[] dialogObjects;
    private GameObject mapCamera;
    private void Awake()
    {
        dialogObjects = GameObject.FindGameObjectsWithTag("Dialog Object");
        mapCamera = GameObject.Find("MapCamera");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (GameObject dialogItem in dialogObjects)
        {
            dialogItem.GetComponentInChildren<CanvasManager>().CloseDialog();
        }
    }

    public void SelectButtonClicked()
    {
        Debug.Log("Select Button Clicked");
    }
    public void NorthButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(0, 1);
    }
    public void EastButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(1, 0);
    }
    public void SouthButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(0, -1);
    }
    public void WestButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(-1, 0);
    }
    public void MapScreenClose()
    {
        Debug.Log("Map Screen Closed");
        MapScreen.SetActive(false);
        Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }
}
