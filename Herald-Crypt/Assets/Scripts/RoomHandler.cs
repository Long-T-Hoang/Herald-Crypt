using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//credits for making this go to this youtube series: https://www.youtube.com/watch?v=qAf9axsyijY&t=1s
//this code will be modified in the future with more features.

public class RoomHandler : MonoBehaviour
{
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    private float totalWidth;
    private float totalHeight;
    private float lowestX = 0;
    private float lowestY = 0;
    private float highestX = 0;
    private float highestY = 0;
    private GameObject[] objects;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.2f);
    }


    void Spawn()
    {
        if (spawned == false)
        {
            if (openingDirection == 1)
            {
                // bottom door.
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 2)
            {
                // top door.
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 3)
            {
                // left door.
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 4)
            {
                // right door.
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
            }
            spawned = true;
        }
        objects = GameObject.FindGameObjectsWithTag("room");

        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].transform.position.x < lowestX)
            {
                lowestX = objects[i].transform.position.x;
            }
            if (objects[i].transform.position.y < lowestY)
            {
                lowestY = objects[i].transform.position.y;
            }
            if (objects[i].transform.position.x > highestX)
            {
                highestX = objects[i].transform.position.x;
            }
            if (objects[i].transform.position.y > highestY)
            {
                highestY = objects[i].transform.position.y;
            }

            totalWidth = highestX - lowestX;
            totalHeight = highestY - lowestY;

            Debug.Log("Lowest: " + lowestX + " " + lowestY + " Highest: " + highestX + " " + highestY + " Width: " + totalWidth + " Height: " + totalHeight);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            Destroy(gameObject);
        }
    }
}
