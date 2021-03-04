using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTING : MonoBehaviour
{
    private Pathfinding pathFinding;

    // Start is called before the first frame update
    void Start()
    {
        pathFinding = new Pathfinding(10, 10, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathFinding.NodeGrid.WorldToCellPos(clickPos, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath(0, 0, x, y);

            if(path != null)
            {
                for(int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y), new Vector3(path[i + 1].x, path[i + 1].y), Color.green, 100f);
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathNode currentNode = pathFinding.NodeGrid.GetGridObject(clickPos);
            currentNode.ToggleWalkable();
            pathFinding.NodeGrid.UpdateValue(currentNode.x, currentNode.y);
        }
    }
}
