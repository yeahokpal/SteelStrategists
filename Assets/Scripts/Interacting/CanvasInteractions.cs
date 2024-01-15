/*
 * Programmers: Caden Mesina
 * Purpose: Handles interactions such as closing dialog boxes and contains button click methods
 * Input: Player clicks and button presses
 * Output: N/A
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasInteractions : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    [SerializeField] private GameObject mapScreen;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject botIcon1;
    [SerializeField] private GameObject botIcon2;
    [SerializeField] private GameObject botIcon3;
    [SerializeField] private GameManager gm;
    private List<GameObject> botIconList = new List<GameObject>();
    private GameObject[] dialogObjects;
    private GameObject mapCamera;
    private MapManager mapManager;
    public int selectedBotNum;
    public TileType selectedTileType = TileType.None;
    public GameObject overlay;
    public GameObject[] RobotSprites;
    #endregion

    #region Default Methods
    private void Awake()
    {
        dialogObjects = GameObject.FindGameObjectsWithTag("Dialog Object");
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
        mapCamera = GameObject.Find("MapCamera");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        botIconList.Add(botIcon1);
        botIconList.Add(botIcon2);
        botIconList.Add(botIcon3);
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
    /* 
     * the next 4 methods happen when players press a specific direction on the map screen
     * they trigger MoveSelector(int, int) from MapManager.cs which moves the position of the
     * selected tile on the map
     */
    //when player presses the north button on the map screen
    public void NorthButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(0, 1);
    }
    //when player presses the east button on the map screen
    public void EastButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(1, 0);
    }
    //when player presses the south button on the map screen
    public void SouthButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(0, -1);
    }
    //when player presses the west button on the map screen
    public void WestButtonClicked()
    {
        GameObject.Find("Map").GetComponent<MapManager>().MoveSelector(-1, 0);
    }
    public void MapScreenClose()
    {
        Debug.Log("Map Screen Closed");
        mapScreen.SetActive(false);
        if (player == null) Debug.Log("player is null");
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }
    // Called when the start button on the Map Screen UI is clicked
    public void StartButtonClicked()
    {
        // Making it so that you cant send out this bot until you gather its materials
        GameObject button = GameObject.Find("Robot" + selectedBotNum + "Button");
        button.GetComponent<Image>().color = Color.black;
        button.GetComponent<Button>().interactable = false;

        // Deselecting from the current button
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.GetComponent<Button>().interactable = false;
     
        gm.UpdateBot(selectedBotNum, selectedTileType);
        botIconList[selectedBotNum - 1].transform.position = mapManager.mapTiles.transform.GetChild(mapManager.mapGrid[mapManager.selectedY, mapManager.selectedX].getIndex()).transform.position;

        selectedBotNum = 0;
    }
    // Called whenever a bot is clicked on the Map Screen UI
    public void UpdateSelectedBot(int botNum)
    {
        selectedBotNum = botNum;
        //visually show which bot is selected
        overlay.transform.position = RobotSprites[botNum - 1].transform.position;

        GameObject.Find("Map").GetComponent<MapManager>().UpdateSelectedTile();
    }
    #endregion
}
