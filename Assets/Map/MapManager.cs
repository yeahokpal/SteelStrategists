using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject mapTiles;
    [SerializeField] private GameObject mapSelector;
    [NonSerialized] public int selectedX = 7;
    [NonSerialized] public int selectedY = 7;
    private MapTile[,] mapGrid = new MapTile[15, 15];
    private System.Random random = new System.Random();
    private int[] mapChance = 
        {4,4,4,4,4,5,5,5,5,5,6,6,6,6,6,7,7,7,7,7,
        8,8,9,9,10,10,11,11,
        12,13,14,15,
        16,16,17,17,18,18,19,19};
    private Sprite[] mapSprites;

    private void Awake()
    {
        mapSprites = Resources.LoadAll<Sprite>("Sprites/MapIcons");
        if (mapSprites == null)
        {
            Debug.Log("Map tiles not found");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
        for (int i = 0; i < mapGrid.GetLength(0); i++)
        {
            for (int j = 0; j < mapGrid.GetLength(1); j++)
            {
                mapGrid[i, j] = new MapTile(i, j, count, mapSprites[mapChance[random.Next(0, mapChance.Length)]]);
                mapTiles.transform.GetChild(count).GetComponent<SpriteRenderer>().sprite = mapGrid[i, j].getSprite();
                count++;
            }
        }
        mapGrid[7, 7].setSprite(mapSprites[random.Next(0,3)]);
        mapTiles.transform.GetChild(mapGrid[7, 7].getIndex()).GetComponent<SpriteRenderer>().sprite = mapGrid[7, 7].getSprite();
    }
    public Sprite GetSpriteByName(string name)
    {
        Sprite output = null;
        for (int i = 0; i < mapSprites.Length; i++)
        {
            if (mapSprites[i].name == name) output = mapSprites[i];
            else Debug.Log("Map Sprite not Found");
        }
        return output;
    }
    public void MoveSelector(int xDistance, int yDistance)
    {
        if (selectedX > 0 && xDistance < 0) selectedX = selectedX + xDistance;
        if (selectedX < 14 && xDistance > 0) selectedX = selectedX + xDistance;
        if (selectedY > 0 && yDistance > 0) selectedY = selectedY - yDistance;
        if (selectedY < 14 && yDistance < 0) selectedY = selectedY - yDistance;
        Debug.Log("Map Coordinates: Selected X = " + selectedX + ".  Selected Y = " + selectedY + ".");
        mapSelector.GetComponentInChildren<Transform>().position = mapTiles.transform.GetChild(mapGrid[selectedY, selectedX].getIndex()).GetComponent<Transform>().position;
    }
}
