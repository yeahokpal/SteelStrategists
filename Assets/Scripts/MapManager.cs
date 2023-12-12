using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject mapTiles;
    private MapTile[,] mapGrid = new MapTile[15,15];
    private System.Random random = new System.Random();
    private int[] mapChances = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,4,4,4,4,4,3,3,3,3,2,2};
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
                mapGrid[i, j] = new MapTile(i, j, count, mapSprites[mapChances[random.Next(0, mapChances.Length)]]);
                mapTiles.transform.GetChild(count).GetComponent<SpriteRenderer>().sprite = mapGrid[i, j].getSprite();
                count++;
            }
        }
        mapGrid[7, 7].setSprite(mapSprites[0]);
        Debug.Log("Center Tile: " + mapGrid[7, 7].getSpriteName() + " Index: " + mapGrid[7, 7].getIndex());
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
}
