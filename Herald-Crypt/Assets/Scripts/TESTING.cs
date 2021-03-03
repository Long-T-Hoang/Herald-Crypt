using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTING : MonoBehaviour
{
    private Grid grid;

    public bool debugOn;

    // Start is called before the first frame update
    void Start()
    {
        if (!debugOn) return;

        grid = new Grid(10, 10, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!debugOn) return;

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.WorldToCellPos(clickPos, out int x, out int y);
            grid.ToggleValue(x, y);
        }
    }
}
