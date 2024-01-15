/*
 * Programmer: Caden Mesina
 * Purpose: Creates a multidimensional array of MapTiles which correspond to a map tile gameobject in scene.
 * Also constains MoveSelector(int, int) to change which tile is selected
 * Input: All code is executed on awake or start except for MoveSelector(int, int) which is called in
 * CanvasInteractions.cs when a button on the map screen is pressed
 * Output: 225 MapTile objects and the randomly generated map made up of those MapTile objects
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour
{
    #region Global Variables
    public GameObject mapTiles;
    [SerializeField] private GameObject mapSelector;
    [SerializeField] TextMeshProUGUI selectedTypeText;
    [SerializeField] TextMeshProUGUI availableResources;
    [NonSerialized] public int selectedX = 7;
    [NonSerialized] public int selectedY = 7;
    public readonly MapTile[,] mapGrid = new MapTile[15, 15];
    private System.Random random = new System.Random();
    public GameObject startButton;
    //this controls the odds of each tile being generated
    private int[] mapChance = 
        {4,4,4,4,4,5,5,5,5,5,6,6,6,6,6,7,7,7,7,7, //4-7 are grass tiles.  Most likely to be generated
        8,8,9,9,10,10,11,11, //8-11 are stone tiles
        12,13,14,15, //12-15 are water tiles
        16,16,17,17,18,18,19,19}; //16-19 are sand tiles
    private Sprite[] mapSprites; //list of all the map sprites
    #endregion

    #region Default Methods
    private void Awake()
    {
        startButton.GetComponent<Button>().interactable = false;
        mapSprites = Resources.LoadAll<Sprite>("Sprites/MapIcons"); //loads all of the sprites from Assets/Resources/Sprites/MapIcons" into mapSprites
        if (mapSprites == null)
        {
            Debug.Log("Map tiles not found");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
        //loops through mapGrid to create MapTile objects for every available spot
        for (int i = 0; i < mapGrid.GetLength(0); i++)
        {
            for (int j = 0; j < mapGrid.GetLength(1); j++)
            {
                //creates the MapTile
                mapGrid[i, j] = new MapTile(i, j, count, mapSprites[mapChance[random.Next(0, mapChance.Length)]]);
                //assigns the sprite that the MapTile object has to its corresponding GameObject in scene
                mapTiles.transform.GetChild(count).GetComponent<SpriteRenderer>().sprite = mapGrid[i, j].getSprite();
                count++;

                //finding the correct material to assign to the space
                if (!(i == 7 && j == 7)) //ignoring the house
                {
                    switch (mapGrid[i, j].getSprite().name)
                    {
                        //stacking cases like this acts like OR
                    
                        case "MapIcons_4":
                        case "MapIcons_5":
                        case "MapIcons_6":
                        case "MapIcons_7":
                            //Debug.Log("Map Tile: (" + i + ", " + j + ") Set to Grass");
                            mapGrid[i, j].setTileType(TileType.Grass);
                            break;
                        case "MapIcons_8":
                        case "MapIcons_9":
                        case "MapIcons_10":
                        case "MapIcons_11":
                            //Debug.Log("Map Tile: (" + i + ", " + j + ") Set to Rock");
                            mapGrid[i, j].setTileType(TileType.Rock);
                            break;
                        case "MapIcons_12":
                        case "MapIcons_13":
                        case "MapIcons_14":
                        case "MapIcons_15":
                            //Debug.Log("Map Tile: (" + i + ", " + j + ") Set to Water");
                            mapGrid[i, j].setTileType(TileType.Water);
                            break;
                        case "MapIcons_16":
                        case "MapIcons_17":
                        case "MapIcons_18":
                        case "MapIcons_19":
                            //Debug.Log("Map Tile: (" + i + ", " + j + ") Set to Desert");
                            mapGrid[i, j].setTileType(TileType.Desert);
                            break;
                    }
                }
            }
        }
        //makes the center tile MapTile one of the 4 available base sprites
        mapGrid[7, 7].setSprite(mapSprites[random.Next(0,3)]);
        //updates the center tile in scene to reflect the center MapTile
        mapTiles.transform.GetChild(mapGrid[7, 7].getIndex()).GetComponent<SpriteRenderer>().sprite = mapGrid[7, 7].getSprite();
    }
    #endregion

    #region Custom Methods
    /*
     * Moves the selected tile according to which button is pressed by the player
     * parameter xDistance: an int that says whether or not to move the selected tile in a positive or negative x direction
     * parameter yDistance: an in that says whether or not to move the selected tile in a positive or negative y direction
     * return: none, method is void
     */
    public void MoveSelector(int xDistance, int yDistance)
    {
        //the next 4 if statements check to see if the selected tile is one of the outside tiles and then checks if they're trying to move in a possible direction
        if (selectedX > 0 && xDistance < 0) selectedX = selectedX + xDistance;
        if (selectedX < 14 && xDistance > 0) selectedX = selectedX + xDistance;
        if (selectedY > 0 && yDistance > 0) selectedY = selectedY - yDistance;
        if (selectedY < 14 && yDistance < 0) selectedY = selectedY - yDistance;
        Debug.Log("Map Coordinates: Selected X = " + selectedX + ".  Selected Y = " + selectedY + ".");
        //updates the selected tile to the tile that is specified by selectedX and selectedY
        mapSelector.GetComponentInChildren<Transform>().position = mapTiles.transform.GetChild(mapGrid[selectedY, selectedX].getIndex()).GetComponent<Transform>().position;

        UpdateSelectedTile();
    }

    // Updating UI based on what is currently selected
    public void UpdateSelectedTile()
    {
        //finding the currently selected tile's typing
        switch (mapGrid[selectedY, selectedX].getTileType())
        {
            case TileType.Grass:
                startButton.GetComponent<CanvasInteractions>().selectedTileType = TileType.Grass;
                selectedTypeText.text = "Grass";
                if (startButton.GetComponent<CanvasInteractions>().selectedBotNum != 0) startButton.GetComponent<Button>().interactable = true;
                availableResources.text = "Available Resources: Wood";
                break;
            case TileType.Rock:
                startButton.GetComponent<CanvasInteractions>().selectedTileType = TileType.Rock;
                selectedTypeText.text = "Rock";
                if (startButton.GetComponent<CanvasInteractions>().selectedBotNum != 0) startButton.GetComponent<Button>().interactable = true;
                availableResources.text = "Available Resources: Steel";
                break;
            case TileType.Water:
                startButton.GetComponent<CanvasInteractions>().selectedTileType = TileType.Water;
                selectedTypeText.text = "Water";
                if (startButton.GetComponent<CanvasInteractions>().selectedBotNum != 0) startButton.GetComponent<Button>().interactable = true;
                availableResources.text = "Available Resources: Wood or Steel";
                break;
            case TileType.Desert:
                startButton.GetComponent<CanvasInteractions>().selectedTileType = TileType.Desert;
                selectedTypeText.text = "Desert";
                if (startButton.GetComponent<CanvasInteractions>().selectedBotNum != 0) startButton.GetComponent<Button>().interactable = true;
                availableResources.text = "Available Resources: Electronics";
                break;
            case TileType.None:
                startButton.GetComponent<CanvasInteractions>().selectedTileType = TileType.None;
                selectedTypeText.text = "Home";
                startButton.GetComponent<Button>().interactable = false;
                availableResources.text = "Available Resources: None";
                break;
        }
        Debug.Log("Current Selected Tile: " + mapGrid[selectedY, selectedX].getTileType().ToString());
    }
    #endregion
}
