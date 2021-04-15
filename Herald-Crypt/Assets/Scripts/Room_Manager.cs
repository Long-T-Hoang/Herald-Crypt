using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviour {
    //public int gridLength;

    public List<GameObject> capRooms;       //0-TC, 1-BC, 2-LC, 3-RC
    public List<GameObject> cornerRooms;    //0-TL, 1-TR, 2-BL, 3-BR
    public List<GameObject> edgeRooms;      //0-TE, 1-BE, 2-LE, 3-RE
    public List<GameObject> junctRooms;     //0-TLR, 1-BLR, 2-LTB, 3-RTB
    public List<GameObject> pillarRooms;    //0-HP, 1-VP

    public GameObject startRoom;
    public GameObject centerRoom;

    private Vector2 roomSize = new Vector2(5.5f, 5.5f);

    private GameObject[,] roomGrid;

    public Vector2 lowest = new Vector2(999, 999);
    public Vector2 highest = new Vector2 (-999, -999);

    void Start() {
        CreateStringGrid();
    }

    private void CreateStringGrid() {
        //string[,] stringGrid = new string[gridLength, gridLength];

        string[,] stringGrid = new string[3,3] {
            {"-", "R", "-"},
            {"R", "S", "R"},
            {"R", "R", "R"}
        };

        CreateRoomGrid(stringGrid, 3, 3);
    }

    private void CreateRoomGrid(string [,] pStr, int pW, int pH) {

        roomGrid = new GameObject[3, 3] {
            {null, capRooms[0], null},
            {cornerRooms[0], startRoom, cornerRooms[1]},
            {cornerRooms[2], junctRooms[0], cornerRooms[3]}
        };

        for (int y = 0; y < roomGrid.GetLength(1); y++) {
            for (int x = 0; x < roomGrid.GetLength(0); x++) {
                if (roomGrid[y, x] != null) {
                    Instantiate(roomGrid[y, x], new Vector3(x * roomSize[0], 0 - y * roomSize[1], 0), Quaternion.identity);

                    lowest[0] = Mathf.Min(lowest[0], x * roomSize[0]);
                    lowest[1] = Mathf.Min(lowest[1], 0 - y * roomSize[1]);

                    highest[0] = Mathf.Max(highest[0], x * roomSize[0]);
                    highest[1] = Mathf.Max(highest[1], 0 - y * roomSize[1]);
                }
            }
        }



        /*
        for (int y = 0; y < roomGrid.Length(); y++) {
            for (int x = 0; x < roomGrid.Length(); x++) {
                if (pStr[x, y] != "-"){
                    if (x == 0) {
                        //  Part - TL Corner
                        if (y == 0) {
                            string str = pStr[x, y] + pStr[x + 1, y] + " " + pStr[x, y + 1];
                            
                        }
                    }
                }
            }
        }
        */
    }
}