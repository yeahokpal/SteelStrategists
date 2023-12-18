/*
 * Programmer: Caden Mesina
 * Purpose: used to store info about the map tiles, also potentially allows easier saving of 
 * map layouts if that is something we implement
 * Input: none
 * Output: none
 */
using UnityEngine;

public class MapTile
{
    public int xPos;
    public int yPos;
    private int num;
    private Sprite img;
    public enum TileStatus { Unharvested, Occupied, Harvested };
    public enum TileType { Grass, Rock, Water, Desert };
    public MapTile(int xPosition, int yPosition, int index, Sprite image)
    {
        xPos = xPosition;
        yPos = yPosition;
        img = image;
        num = index;
    }
    public int getIndex()
    {
        return num;
    }
    public Sprite getSprite()
    {
        return img;
    }
    public string getSpriteName()
    {
        return img.name;
    }
    public void setSprite(Sprite image)
    {
        img = image;
    }
}
