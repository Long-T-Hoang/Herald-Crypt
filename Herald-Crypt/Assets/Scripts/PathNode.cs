// Author: Long Hoang
// PathNode to be used with Pathfinding

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;
    public Vector2 cellPos;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode prevNode;

    // Constructor
    public PathNode(Grid<PathNode> grid, int x, int y, bool isWalkable = true)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        cellPos = new Vector2(x, y);
        this.isWalkable = isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ToggleWalkable()
    {
        isWalkable = !isWalkable;
    }

    public override string ToString()
    {
        return isWalkable.ToString();
    }

    public Vector3 GetPos()
    {
        return grid.CellToWorldPos(x, y);
    }
}
