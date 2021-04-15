using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Spawn()
    {
        roomManagerScript.CreateStringGrid();

        //roomStat.CalculateStat();
        pathfinding = new Pathfinding(3 * ROOM_WIDTH_IN_TILE, 3 * ROOM_WIDTH_IN_TILE, 0.5f, roomManagerScript.lowest);

        List<GameObject> rooms = roomManagerScript.roomList;

        enemies = SpawnObjects(rooms, enemyPref, 0, 3);
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
            loadWinScreen();
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
    private List<GameObject> SpawnObjects(List<GameObject> rooms, GameObject[] prefabList, int min, int max)
    {
        List<GameObject> objectList = new List<GameObject>();

        for (int i = 0; i < rooms.Count; i++)
        {
            int objectCount = Random.Range(min, max);

            for (int j = 0; j < objectCount; j++)
            {
                float x = Random.Range(-1.75f, 1.75f);
                float y = Random.Range(-1.75f, 1.75f);
                int randInt = Random.Range(0, prefabList.Length);

                Vector3 position = rooms[i].transform.position + new Vector3(x, y, 0.0f);

                GameObject instance = Instantiate(prefabList[randInt], position, Quaternion.identity, this.transform);
                
                objectList.Add(instance);
            }
        }

        return objectList;
    }

    public void loadWinScreen()
    {
        SceneManager.LoadScene(2);
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
