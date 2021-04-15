using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject[] enemyPref;
    public GameObject[] weaponPref;

    RoomStats roomStat;
    List<GameObject> enemies;
    List<GameObject> weapons;
    Pathfinding pathfinding;
    bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        roomStat = Resources.FindObjectsOfTypeAll<RoomStats>()[0];
        enemies = new List<GameObject>();
        weapons = new List<GameObject>();
        spawned = false;

        roomStat.ResetStat();

        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(4.0f);

        if (roomStat.finishGeneration && !spawned)
        {
            roomStat.CalculateStat();
            pathfinding = new Pathfinding(roomStat.getWidthInTileNum(), roomStat.getHeightInTileNum(), 0.5f, roomStat.getLeftBottomPos());

            GameObject[] rooms = roomStat.getObjectList();

            SpawnEnemies(rooms);
            SpawnObjects(rooms, weaponPref, weapons, 0, 3);
            spawned = true;
        }
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

    private void SpawnEnemies(GameObject[] rooms)
    {
        for(int i = 0; i < rooms.Length; i++)
        {
            int enemyCount = Random.Range(0, 3);

            for (int j = 0; j < enemyCount; j++)
            {
                float x = Random.Range(-1.75f, 1.75f);
                float y = Random.Range(-1.75f, 1.75f);
                int randInt = Random.Range(0, enemyPref.Length);

                Vector3 position = rooms[i].transform.position + new Vector3(x, y, 0.0f);

                GameObject enemyInstance = Instantiate(enemyPref[randInt], position, Quaternion.identity, this.transform);
                enemyInstance.GetComponent<EnemyPathfinding>().pathFinding = pathfinding;
                enemies.Add(enemyInstance);
            }
        }
    }

    private void SpawnWeapons(GameObject[] rooms)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            int weaponCount = Random.Range(0, 3);

            for (int j = 0; j < weaponCount; j++)
            {
                float x = Random.Range(-1.75f, 1.75f);
                float y = Random.Range(-1.75f, 1.75f);
                int randInt = Random.Range(0, weaponPref.Length);

                Vector3 position = rooms[i].transform.position + new Vector3(x, y, 0.0f);

                GameObject weaponInstance = Instantiate(weaponPref[randInt], position, Quaternion.identity, this.transform);
                weaponInstance.GetComponent<EnemyPathfinding>().pathFinding = pathfinding;
                weapons.Add(weaponInstance);
            }
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
    private void SpawnObjects(GameObject[] rooms, GameObject[] prefabList, List<GameObject> objectList, int min, int max)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            int objectCount = Random.Range(min, max);

            for (int j = 0; j < objectCount; j++)
            {
                float x = Random.Range(-1.75f, 1.75f);
                float y = Random.Range(-1.75f, 1.75f);
                int randInt = Random.Range(0, prefabList.Length);

                Vector3 position = rooms[i].transform.position + new Vector3(x, y, 0.0f);

                GameObject instance = Instantiate(prefabList[randInt], position, Quaternion.identity, this.transform);
                //instance.GetComponent<EnemyPathfinding>().pathFinding = pathfinding;
                objectList.Add(instance);
            }
        }
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
