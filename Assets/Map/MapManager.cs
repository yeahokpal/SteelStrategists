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

public class MapManager : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private GameObject mapTiles;
    [SerializeField] private GameObject mapSelector;
    [NonSerialized] public int selectedX = 7;
    [NonSerialized] public int selectedY = 7;
    private MapTile[,] mapGrid = new MapTile[15, 15];
    private System.Random random = new System.Random();
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
    }
    #endregion
}
