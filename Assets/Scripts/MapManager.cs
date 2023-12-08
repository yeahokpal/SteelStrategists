using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private MapTile[,] mapGrid = new MapTile[21,21];
    private System.Random random = new System.Random();
    private int[] mapChances = {1,1,1,1,1,1,1,2,2,2,2,2,3,3,3,3,4,4,4,5,5};
    private Sprite[] mapSprites;

    private void Awake()
    {
        mapSprites = Resources.LoadAll<Sprite>("Sheet Name");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Map Start");
        for (int i = 0; i < mapGrid.GetLength(0); i++)
        {
            for (int j = 0; j < mapGrid.GetLength(1); j++)
            {
                mapGrid[i, j] = new MapTile(i, j, mapSprites[mapChances[random.Next(0, 20)]]);
            }
        }
        /*grid[0, 0] = 1;
        grid[20, 20] = 100;
        Debug.Log(grid.GetLength(0) + ", " + grid.GetLength(1));
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = i; j < grid.GetLength(1); j++)
            {
                grid[i, j] = mapChances[random.Next(0, 20)];
                Debug.Log(grid[i, j]);
            }
        }
        grid[10, 10] = 0;
        Debug.Log("Center Tile: " + grid[10, 10]);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int[,] ResizeGrid(int[,] grid, int newSize)
    {
        int[,] temp = new int[newSize, newSize] ;
        return temp;
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
