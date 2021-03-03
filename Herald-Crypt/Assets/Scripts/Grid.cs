using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width, height;
    private float cellSize;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    // Constructor
    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        // Draw cells
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                // Draw cells
                debugTextArray[x,y] = CreateTextObject(gridArray[x, y].ToString(), CellToWorldPos(x, y));
                Debug.DrawLine(CellToWorldPos(x, y) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(x + 1, y) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
                Debug.DrawLine(CellToWorldPos(x, y) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(x, y + 1) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
            }
        }

        Debug.DrawLine(CellToWorldPos(0, height) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(width, height) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
        Debug.DrawLine(CellToWorldPos(width, 0) - new Vector3(cellSize, cellSize) * 0.5f, CellToWorldPos(width, height) - new Vector3(cellSize, cellSize) * 0.5f, Color.white, 100f);
    }

    private Vector3 CellToWorldPos(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void WorldToCellPos(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPos.x + cellSize * 0.5f / cellSize);
        y = Mathf.FloorToInt(worldPos.y + cellSize * 0.5f / cellSize);
    }

    private TextMesh CreateTextObject(string text, Vector3 position, int fontSize = 100)
    {
        GameObject gameObject = new GameObject("Text Object", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.position = position;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.characterSize = 0.05f;
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

        gridArray[x, y] = value;
        debugTextArray[x, y].text = gridArray[x,y].ToString();
    }
    public void ToggleValue(int x, int y)
    {
        if (!(x >= 0 && y >= 0 && x < width && y < height))
            return;

        if (gridArray[x, y] == 1)
        {
            gridArray[x, y] = 0;
        }
        else if (gridArray[x, y] == 0)
        {
            gridArray[x, y] = 1;
        }

        debugTextArray[x, y].text = gridArray[x, y].ToString();
    }
}
