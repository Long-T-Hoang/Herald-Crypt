// Author: Long Hoang
// Create generic grid array
// Has visual debug and functions to convert between world coords and grid coord

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<GridObject>
{
    private int width, height;
    private float cellSize;
    private Vector3 startPoint;

    private GridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private int[,] gridDebugArray;

    // Properties
    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    // Constructor
    public Grid(int width, int height, float cellSize, Vector3 startPoint, Func<Grid<GridObject>, int, int, GridObject> createGridObject = null)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.startPoint = startPoint;

        gridArray = new GridObject[width, height];
        debugTextArray = new TextMesh[width, height];
        gridDebugArray = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Create grid objects
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        ShowDebug();
    }

    public Vector3 CellToWorldPos(int x, int y)
    {
        Vector3 returnPos = new Vector3(x, y) * cellSize + startPoint;
        return returnPos;
    }

    public void WorldToCellPos(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos.x - startPoint.x + cellSize * 0.5f) / cellSize);
        y = Mathf.FloorToInt((worldPos.y - startPoint.y + cellSize * 0.5f) / cellSize);
    }

    public Vector2 WorldToCellPos(Vector3 worldPos)
    {
        WorldToCellPos(worldPos, out int x, out int y);

        return new Vector2(x, y);
    }

    private TextMesh CreateTextObject(string text, Vector3 position, int fontSize = 100)
    {
        GameObject gameObject = new GameObject("Text Object", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.position = position;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.characterSize = 0.005f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.white;
        return textMesh;
    }

    public void SetValue(int x, int y, int value)
    {
        if (!(x >= 0 && y >= 0 && x < width && y < height))
            return;

        //gridDebugArray[x, y] = value;
        //debugTextArray[x, y].text = gridArray[x,y].ToString();
    }
    public void UpdateValue(int x, int y)
    {
        debugTextArray[x, y].text = gridArray[x, y].ToString();
    }

    public GridObject GetGridObject(int x, int y)
    {
        return gridArray[x, y];
    }

    public GridObject GetGridObject(Vector3 pos)
    {
        WorldToCellPos(pos, out int x, out int y);
        return GetGridObject(x, y);
    }

    private void ShowDebug()
    {
        Debug.Log(startPoint);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Draw cells
                debugTextArray[x, y] = CreateTextObject(gridArray[x, y].ToString(), CellToWorldPos(x, y));
                Debug.DrawLine(CellToWorldPos(x, y) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(x + 1, y) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
                Debug.DrawLine(CellToWorldPos(x, y) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(x, y + 1) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
            }
        }

        Debug.DrawLine(CellToWorldPos(0, height) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(width, height) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
        Debug.DrawLine(CellToWorldPos(width, 0) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(width, height) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
    }
}
