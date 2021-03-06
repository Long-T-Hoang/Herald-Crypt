﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemyPref;
    public GameObject[] weaponPref;
    private GameObject roomManager;
    private Room_Manager roomManagerScript;

    private List<GameObject> enemies;
    private List<GameObject> weapons;
    private Pathfinding pathfinding;
    bool spawned;

    const int ROOM_WIDTH_IN_TILE = 11;

    // Start is called before the first frame update
    void Start()
    {
        //roomStat = Resources.FindObjectsOfTypeAll<RoomStats>()[0];
        enemies = new List<GameObject>();
        spawned = false;

        roomManager = GameObject.Find("RoomManager");
        roomManagerScript = roomManager.GetComponent<Room_Manager>();

        // Always put in bottom
        Spawn();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        // Debug for room number generation
        //for(; PlayerSaveStats.Level <= 10; PlayerSaveStats.Level++)
        //{
        //    Debug.Log("level:" + PlayerSaveStats.Level + "roomNum:" + PlayerSaveStats.CalculateRoomNum());
        //}
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Spawn()
    {
        roomManagerScript.CreateStringGrid();

        //roomStat.CalculateStat();
        Vector3 lowest = roomManagerScript.lowest - roomManagerScript.tileSize * 10;
        pathfinding = new Pathfinding(roomManagerScript.roomWidth * ROOM_WIDTH_IN_TILE, roomManagerScript.roomHeight * ROOM_WIDTH_IN_TILE, 0.5f, lowest);

        List<GameObject> rooms = roomManagerScript.roomList;

        enemies = SpawnObjects(rooms, enemyPref, 0, 3, false);
        weapons = SpawnObjects(rooms, weaponPref, 0, 3);

        // Assign pathfinding to enemies
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyPathfinding>().pathFinding = pathfinding;
        }
         
        spawned = true;
    }

    private void LateUpdate()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }

        if (spawned && enemies.Count == 0)
        {
            loadNextLevel();
        }
    }

    /// <summary>
    /// Spawn an object from prefab list in a room between min (inclusive) and max (exclusive)
    /// </summary>
    /// <param name="rooms">List of rooms</param>
    /// <param name="prefabList">Prefab list to choose from</param>
    /// <param name="objectList">Object list to store spawned objects</param>
    /// <param name="min">minimum number of object spawn in a room</param>
    /// <param name="max">maximum number of object spawn in a room</param>
    private List<GameObject> SpawnObjects(List<GameObject> rooms, GameObject[] prefabList, int min, int max, bool spawnInStart = true)
    {
        List<GameObject> objectList = new List<GameObject>();

        for (int i = 0; i < rooms.Count; i++)
        {
            int objectCount = Random.Range(min, max);

            for (int j = 0; j < objectCount; j++)
            {
                float roomHalfSize = (roomManagerScript.roomSize.x / 2) - 0.75f;
                float x = Random.Range(-roomHalfSize, roomHalfSize);
                float y = Random.Range(-roomHalfSize, roomHalfSize);
                int randInt = Random.Range(0, prefabList.Length);
                Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

                Vector3 position = rooms[i].transform.position + new Vector3(x, y, 0.0f);

                if (Vector2.Distance(position, rooms[0].transform.position) < roomManagerScript.roomSize.x / 2 && spawnInStart == false)
                {
                    //Debug.Log("room index:" + i + " name:" + instance.name + " (x,y):" + x + " " + y);
                    break;
                }

                GameObject instance = Instantiate(prefabList[randInt], position, rotation, this.transform);
                instance.name = prefabList[randInt].name;
                objectList.Add(instance);

            }
        }

        return objectList;
    }

    public void loadWinScreen()
    {
        SceneManager.LoadScene(2);
    }

    public void loadNextLevel()
    {
        // Save data for next level
        if(PlayerSaveStats.Level >= 10)
        {
            loadWinScreen();
            return;
        }

        PlayerSaveStats.Level++;
        PlayerSaveStats.Health = player.GetComponent<PlayerStats>().Health;
        List<GameObject> inventory = player.GetComponent<PlayerAttack>().Inventory;

        for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] != null)
            {
                PlayerSaveStats.Inventory[i] = inventory[i].name;
                PlayerSaveStats.InvAmmo[i] = inventory[i].GetComponent<Weapons>().UsedCount;
            }
            else
            {
                PlayerSaveStats.Inventory[i] = "";
                PlayerSaveStats.InvAmmo[i] = 0;
            }
        }

        SceneManager.LoadScene(3);
    }

    public void loadGameOverScreen()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Output a weighted random int from min (inclusive) to max (exclusive)
    /// </summary>
    /// <param name="min">minimum</param> 
    /// <param name="max">maximum</param> 
    /// <param name="indexOfWeighted">weighted number</param>
    /// <param name="weightedness">weightedness in percentage (must be between 1-99)</param>
    /// <returns></returns>
    public int WeightedRandom(int min, int max, int indexOfWeighted, int weightedness)
    {
        int remainingWeight = 100 - weightedness;
        int range = max - min;
        int weightOfOtherNum = remainingWeight / (range - 1);
        Vector2[] weightRange = new Vector2[range];

        int count = 0;

        for (int i = 0; i < range; i++)
        {
            if (i == indexOfWeighted)
            {
                weightRange[i] = new Vector2(count, count += weightedness);
            }
            else
            {
                weightRange[i] = new Vector2(count, count += weightOfOtherNum);
            }
        }

        int rand = Random.Range(0, 100);

        for(int i = 0; i < range; i++)
        {
            if(rand >= weightRange[i].x && rand <= weightRange[i].y)
            {
                return i + min;
            }
        }

        return 0;
    }
}
