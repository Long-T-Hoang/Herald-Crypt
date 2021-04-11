// Author: Long Hoang
// Pathfinding script
// Create pathfinding grid and calculate path using A* 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_COST_ADJACENT = 10;
    private const int MOVE_COST_DIAGONAL = 14;

    private Grid<PathNode> nodeGrid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Grid<PathNode> NodeGrid
    {
        get { return nodeGrid; }
    }

    // Constructor 
    public Pathfinding(int width, int height, float cellSize, Vector3 startPoint)
    {
        nodeGrid = new Grid<PathNode>(width, height, cellSize, startPoint, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

        AddWalls();
    }

    private void AddWalls()
    {
        for(int x = 0; x < nodeGrid.Width; x++)
        {
            for(int y = 0; y < nodeGrid.Height; y++)
            {
                PathNode node = nodeGrid.GetGridObject(x, y);
                Vector3 nodePos = node.GetPos();
                Collider2D collider = Physics2D.OverlapCircle(nodePos, 0.2f);
                if (collider != null && collider.CompareTag("Wall"))
                {
                    node.isWalkable = false;
                }
            }
        }
    }

    // Calculate and return path from startNode to endNode
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = nodeGrid.GetGridObject(startX, startY);
        PathNode endNode = nodeGrid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        // Set all node to make gCost
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

            // Assigned current node to "searched"
            // Remove current node from openList and add to closedList
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                // Continue if node is in closed list or not walkable
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                // Continue if node adjacent to current and neighbor node is not walkable
                if (!AvoidWall(currentNode, neighbourNode))
                {
                    continue;
                }

                // Calculate cost of current neighbour node
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.prevNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    // Add to open list if not on open list
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

        for(int x = -1; x < 2; x++)
        {
            for(int y = -1; y < 2; y++)
            {
                int nodeX = currentNode.x + x;
                int nodeY = currentNode.y + y;
                if (nodeX < 0 || nodeY < 0 || nodeX >= nodeGrid.Width || nodeY >= nodeGrid.Height) continue;

                neighbourList.Add(nodeGrid.GetGridObject(nodeX, nodeY));
            }
        }

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

    // Return false if node adjacent to current and neighbor is not walkable
    private bool AvoidWall(PathNode current, PathNode neighbor)
    {
        // Get mutually adjacent node
        PathNode[] adjacents = new PathNode[2];
        adjacents[0] = nodeGrid.GetGridObject(current.x, neighbor.y);
        adjacents[1] = nodeGrid.GetGridObject(neighbor.x, current.y);

        if(!adjacents[0].isWalkable || !adjacents[1].isWalkable)
        {
            return false;
        }

        return true;
    }
}
