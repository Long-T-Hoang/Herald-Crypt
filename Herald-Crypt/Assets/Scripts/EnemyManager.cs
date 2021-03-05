using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Pathfinding pathFinding;

    // Start is called before the first frame update
    void Start()
    {
        pathFinding = new Pathfinding(20, 20, 1);
    }

    // Update is called once per frame
    void Update()
    {
        ToggleWalkable();
    }

    private void ToggleWalkable()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathNode currentNode = pathFinding.NodeGrid.GetGridObject(clickPos);
            currentNode.ToggleWalkable();
            pathFinding.NodeGrid.UpdateValue(currentNode.x, currentNode.y);
        }
    }
}
