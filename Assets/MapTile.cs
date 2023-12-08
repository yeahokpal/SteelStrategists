using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    private int xPos;
    private int yPos;
    private Sprite img;
    public MapTile(int xPosition, int yPosition, Sprite image)
    {
        xPos = xPosition;
        yPos = yPosition;
        img = image;

    }
    public int getXPosition()
    {
        return xPos;
    }
    public int getYPosition()
    {
        return yPos;
    }
    public Sprite getSprite()
    {
        return img;
    }
}
