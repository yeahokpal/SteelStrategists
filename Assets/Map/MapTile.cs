/*
 * Programmer: Caden Mesina
 * Purpose: used to store info about the map tiles, also potentially allows easier saving of 
 * map layouts if that is something we implement
 * Input: none
 * Output: none
 */
using UnityEngine;
public enum TileStatus { Unharvested, Occupied, Harvested };
public enum TileType { Grass, Rock, Water, Desert, None };

public class MapTile
{
    public int xPos;
    public int yPos;
    private int num;
    private Sprite img;
    private TileStatus tileStatus = TileStatus.Unharvested;
    private TileType tileType = TileType.None;
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
    public void setTileType(TileType type)
    {
        tileType = type;
    }
    public void setTileStatus(TileStatus status)
    {
        tileStatus = status;
    }
    public TileType getTileType()
    {
        return tileType;
    }
    public TileStatus getTileStatus()
    {
        return tileStatus;
    }
}
