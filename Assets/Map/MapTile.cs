using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    private int xPos;
    private int yPos;
    private int num;
    private Sprite img;
    public MapTile(int xPosition, int yPosition, int index, Sprite image)
    {
        xPos = xPosition;
        yPos = yPosition;
        img = image;
        num = index;

    }
    public int getXPosition()
    {
        return xPos;
    }
    public int getYPosition()
    {
        return yPos;
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
