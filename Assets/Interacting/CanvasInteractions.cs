/*
 * Programmers: Caden Mesina
 * Purpose: Handles interactions such as closing dialog boxes and contains button click methods
 * Input: Player clicks and button presses
 * Output: N/A
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CanvasInteractions : MonoBehaviour, IPointerClickHandler
{
    #region Global Variables
    [SerializeField] private GameObject MapScreen;
    [SerializeField] private GameObject Player;
    private GameObject[] dialogObjects;
    private GameObject mapCamera;
    #endregion

    #region Default Methods
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
    #endregion

    #region Custom Methods
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
    #endregion
}
