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
    [SerializeField] GameManager gm;
    private GameObject[] dialogObjects;
    private GameObject mapCamera;
    public int selectedBotNum;
    public TileType selectedTileType = TileType.None;
    #endregion

    #region Default Methods
    private void Awake()
    {
        dialogObjects = GameObject.FindGameObjectsWithTag("Dialog Object");
        mapCamera = GameObject.Find("MapCamera");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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
    //when player presses the select button on the map screen
    public void SelectButtonClicked()
    {
        Debug.Log("Select Button Clicked");
    }
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
        MapScreen.SetActive(false);
        Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }
    public void StartButtonClicked()
    {
        //gm.UpdateBot(selectedBotNum, selectedTileType); -----UNCOMMENT
    }
    public void UpdateSelectedBot(int botNum)
    {
        selectedBotNum = botNum;

        //visually show which bot is selected (do later)
        /*switch (botNum)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }*/
    }
    #endregion
}
