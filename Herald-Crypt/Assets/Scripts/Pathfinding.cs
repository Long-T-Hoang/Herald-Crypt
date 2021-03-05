// Author: Long Hoang
// Pathfinding script
// Create pathfinding grid and calculate path using A* 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_COST = 1;

    private Grid<PathNode> nodeGrid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Grid<PathNode> NodeGrid
    {
        get { return nodeGrid; }
    }

    // Constructor 
    public Pathfinding(int width, int height, float cellSize)
    {
        float startOffset = -width * cellSize * 0.5f + cellSize * 0.5f;

        nodeGrid = new Grid<PathNode>(width, height, cellSize, new Vector3(startOffset, startOffset), (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    // Calculate and return path from startNode to endNode
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = nodeGrid.GetGridObject(startX, startY);
        PathNode endNode = nodeGrid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < nodeGrid.Width; x++)
        {
            for(int y = 0; y < nodeGrid.Height; y++)
            {
                PathNode pathNode = nodeGrid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.prevNode = null;
            }
        }

        // Calculate cost for each nodes
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Search loop
        while(openList.Count > 0)
        {
            PathNode currentNode = getLowestCostNode(openList);

            // Reached final node
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.prevNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    // Add to open list if not on
                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes, cannot find path
        return null;
    }

    // FindPath override with Vector3 param
    public List<PathNode> FindPath(Vector3 start, Vector3 end)
    {
        nodeGrid.WorldToCellPos(start, out int startX, out int startY);
        nodeGrid.WorldToCellPos(end, out int endX, out int endY);
        
        return FindPath(startX, startY, endX, endY);
    }


    // Misc functions
    // Return four adjacent neighbours of currentNode
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        // left
        if (currentNode.x - 1 >= 0)
            neighbourList.Add(nodeGrid.GetGridObject(currentNode.x - 1, currentNode.y));
        // right
        if (currentNode.x + 1 < nodeGrid.Width)
            neighbourList.Add(nodeGrid.GetGridObject(currentNode.x + 1, currentNode.y));
        // up
        if (currentNode.y + 1 < nodeGrid.Height)
            neighbourList.Add(nodeGrid.GetGridObject(currentNode.x, currentNode.y + 1));
        //down
        if (currentNode.y - 1 >= 0)
            neighbourList.Add(nodeGrid.GetGridObject(currentNode.x, currentNode.y - 1));

        return neighbourList;
    }

    // Calculate and return list of nodes to get to endNode
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.prevNode != null)
        {
            path.Add(currentNode.prevNode);
            currentNode = currentNode.prevNode;
        }

        path.Reverse();
        
        return path;
    }

    // Calculate heuristic cost
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);

        return xDist + yDist;
    }

    // Return node with lowest f cost in list
    private PathNode getLowestCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestCostNode = pathNodeList[0];

        for(int i = 1; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestCostNode.fCost)
            {
                lowestCostNode = pathNodeList[i];
            }
        }

        return lowestCostNode;
    }
}
