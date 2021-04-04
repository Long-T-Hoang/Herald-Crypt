using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject enemyPref;

    RoomStats roomStat;
    List<GameObject> enemies;
    Pathfinding pathfinding;
    bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        roomStat = Resources.FindObjectsOfTypeAll<RoomStats>()[0];
        enemies = new List<GameObject>();
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
            SpawnEnemies();
            spawned = true;
        }
    }

    private void LateUpdate()
    {
    }

    private void SpawnEnemies()
    {
        roomStat.CalculateStat();
        pathfinding = new Pathfinding(roomStat.getWidthInTileNum(), roomStat.getHeightInTileNum(), 0.5f, roomStat.getLeftBottomPos());

        GameObject[] rooms = roomStat.getObjectList();

        for(int i = 0; i < rooms.Length; i++)
        {
            GameObject enemyInstance = Instantiate(enemyPref, rooms[i].transform.position, Quaternion.identity, this.transform);
            enemyInstance.GetComponent<EnemyPathfinding>().pathFinding = pathfinding;
            enemies.Add(enemyInstance);
        }
    }

    private void SpawnWeapons()
    {

    }
}
