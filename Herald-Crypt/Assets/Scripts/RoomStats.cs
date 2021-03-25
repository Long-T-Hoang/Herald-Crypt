using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomStats", menuName = "ScriptableObjects/RoomStats", order = 1)]
public class RoomStats : ScriptableObject
{
    public bool finishGeneration = false;

    private int totalWidth;
    private int totalHeight;
    private float lowestX = 0;
    private float lowestY = 0;
    private float highestX = 0;
    private float highestY = 0;
    private GameObject[] objects;

    public void ResetStat()
    {
        lowestX = 0;
        lowestY = 0;
        highestX = 0;
        highestY = 0;
    }

    public void CalculateStat()
    {
        objects = GameObject.FindGameObjectsWithTag("room");

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.position.x < lowestX)
            {
                lowestX = objects[i].transform.position.x - 5.0f / 2;
            }
            if (objects[i].transform.position.y < lowestY)
            {
                lowestY = objects[i].transform.position.y - 5.0f / 2;
            }
            if (objects[i].transform.position.x > highestX)
            {
                highestX = objects[i].transform.position.x + 5.0f / 2;
            }
            if (objects[i].transform.position.y > highestY)
            {
                highestY = objects[i].transform.position.y + 5.0f / 2;
            }

            totalWidth = (int)((highestX - lowestX) * 2 + 1);
            totalHeight = (int)((highestY - lowestY) * 2 + 1);

            // Logging stat of generated room
            //Debug.Log("Lowest: " + lowestX + " " + lowestY + " Highest: " + highestX + " " + highestY + " Width: " + totalWidth + " Height: " + totalHeight);
        }

        finishGeneration = true;
    }

    public Vector3 getLeftBottomPos()
    {
        return new Vector3(lowestX, lowestY);
    }

    public int getWidthInTileNum()
    {
        return totalWidth;
    }
    public int getHeightInTileNum()
    {
        return totalHeight;
    }

    public GameObject[] getObjectList()
    {
        return objects;
    }
}
