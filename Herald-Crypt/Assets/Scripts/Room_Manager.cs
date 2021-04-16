using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviour {
    public int gridLength;

    public List<GameObject> capRooms;       //0-TC, 1-BC, 2-LC, 3-RC
    public List<GameObject> cornerRooms;    //0-TL, 1-TR, 2-BL, 3-BR
    public List<GameObject> edgeRooms;      //0-TE, 1-BE, 2-LE, 3-RE
    public List<GameObject> junctRooms;     //0-TLR, 1-BLR, 2-LTB, 3-RTB
    public List<GameObject> pillarRooms;    //0-HP, 1-VP

    public GameObject startRoom;
    public GameObject centerRoom;

    private Vector2 roomSize = new Vector2(5.5f, 5.5f);

    public GameObject player;
    private GameObject[,] roomGrid;

    // Stats for generating grids for pathfinding
    public Vector2 lowest = new Vector2(999, 999);
    public Vector2 highest = new Vector2 (-999, -999);
    public List<GameObject> roomList;

    void Start() {
        //CreateStringGrid();

        roomList = new List<GameObject>();
    }

    private void CreateStringGrid() {
        //  Part - Create String Grid
        if (gridLength < 3) {
            gridLength = 3;
        }

        string[,] stringGrid = new string[3,3] {
            {"-", "r", "-"},
            {"r", "r", "r"},
            {"r", "r", "r"}
        };

        // Part - Setup String Grid
        /*
        string[,] stringGrid = new string[gridLength, gridLength];
        for (int y = 0; y < stringGrid.GetLength(1); y++) {
            for (int x = 0; x < stringGrid.GetLength(0); x++) {
                
            }
        }
        */

        CreateRoomGrid(stringGrid, stringGrid.GetLength(0), stringGrid.GetLength(1));
    }

    private void CreateRoomGrid(string [,] pStr, int pW, int pH) {
        /*
        roomGrid = new GameObject[3, 3] {
            {null, capRooms[0], null},
            {cornerRooms[0], startRoom, cornerRooms[1]},
            {cornerRooms[2], junctRooms[0], cornerRooms[3]}
        };
        */

        // Part - Create String Grid, Setup Start Room, Move Player
        roomGrid = new GameObject[pW, pH];

        Vector2 halfPoint = new Vector2(roomGrid.GetLength(0) / 2, roomGrid.GetLength(1) / 2);
        roomGrid[(int)halfPoint[0], (int)halfPoint[1]] = startRoom;

        Instantiate(roomGrid[(int)halfPoint[0], (int)halfPoint[1]], new Vector3(halfPoint[0] * roomSize[0], 0 - halfPoint[1] * roomSize[1], 0), Quaternion.identity);

        player.transform.position = new Vector3(halfPoint[0] * roomSize[0], 0 - halfPoint[1] * roomSize[1], 0);

        // Part - Setup Room Grid
        string roomStr = "";

        Debug.Log(roomGrid.GetLength(0) + ", " + roomGrid.GetLength(1));

        for (int y = 0; y < roomGrid.GetLength(1); y++) {
            for (int x = 0; x < roomGrid.GetLength(0); x++) {
                if (roomGrid[y, x] == null) {
                    //  Part - Top Row
                    if (y == 0) {
                        //  SubPart - TL Corner
                        if (x == 0) {
                            Debug.Log("TL Corner");
                            roomStr = pStr[x, y] + pStr[x + 1, y] + " " + pStr[x, y + 1];
                            roomGrid[y, x] = CR_Corner(1, roomStr);
                        }

                        //  SubPart - T Edge
                        else if (x > 0 && x < roomGrid.GetLength(0) - 1) {
                            roomStr = pStr[x - 1, y] + pStr[x, y] + pStr[x + 1, y] + " " + pStr[x, y + 1];
                            roomGrid[y, x] = CR_Edge(1, roomStr);
                        }

                        //  SubPart - TR Corner
                        else if (x == roomGrid.GetLength(0) - 1) {
                            roomStr = pStr[x - 1, y] + pStr[x, y] + " " + pStr[x, y + 1];
                            roomGrid[y, x] = CR_Corner(2, roomStr);
                        }
                    }

                    //  Part - Central Rows
                    else if (y > 0 && y < roomGrid.GetLength(1) - 1) {
                        //  SubPart - L Edge
                        if (x == 0) {
                            roomStr = pStr[x, y - 1] + pStr[x, y] + pStr[x, y + 1] + " " + pStr[x + 1, y];
                            roomGrid[y, x] = CR_Edge(3, roomStr);
                        }

                        //  SubPart - Central
                        else if (x > 0 && x < roomGrid.GetLength(0) - 1) {
                            roomStr = pStr[x - 1, y] + " " + pStr[x, y - 1] + pStr[x, y] + pStr[x, y + 1] + " " + pStr[x + 1, y];
                            roomGrid[y, x] = CR_Center(roomStr);
                        }

                        //  SubPart - R Edge
                        else if (x == roomGrid.GetLength(0) - 1) {
                            roomStr = pStr[x, y - 1] + pStr[x, y] + pStr[x, y + 1] + " " + pStr[x - 1, y];
                            roomGrid[y, x] = CR_Edge(4, roomStr);
                        }
                    }

                    //  Part - Bottom Row
                    else if (y == roomGrid.GetLength(1) - 1) {
                        //  SubPart - BL Corner
                        if (x == 0) {
                            roomStr = pStr[x, y] + pStr[x + 1, y] + " " + pStr[x, y - 1];
                            roomGrid[y, x] = CR_Corner(3, roomStr);
                        }

                        //  SubPart - B Edge
                        else if (x > 0 && x < roomGrid.GetLength(0) - 1) {
                            roomStr = pStr[x - 1, y] + pStr[x, y] + pStr[x + 1, y] + " " + pStr[x, y - 1];
                            roomGrid[y, x] = CR_Edge(2, roomStr);
                        }

                        //  SubPart - BR Corner
                        else if (x == roomGrid.GetLength(0) - 1) {
                            roomStr = pStr[x - 1, y] + pStr[x, y] + " " + pStr[x, y - 1];
                            roomGrid[y, x] = CR_Corner(4, roomStr);
                        }
                    }

                    if (roomGrid[y, x] != null) {
                        Instantiate(roomGrid[y, x], new Vector3(x * roomSize[0], 0 - y * roomSize[1], 0), Quaternion.identity);

                        lowest[0] = Mathf.Min(lowest[0], x * roomSize[0]);
                        lowest[1] = Mathf.Min(lowest[1], 0 - y * roomSize[1]);

                        highest[0] = Mathf.Max(highest[0], x * roomSize[0]);
                        highest[1] = Mathf.Max(highest[1], 0 - y * roomSize[1]);
                    }
                }
            }
        }
    }

    private GameObject CR_Corner(int pType, string pStr) {
        switch(pType) {
            //  Part - TL Corner
            case 1:
                switch(pStr) {
                    case "rr r":
                        return cornerRooms[0];

                    case "rr -":
                        return capRooms[2];

                    case "r- r":
                        return capRooms[0];
                }
                break;

            //  Part - TR Corner
            case 2:
                switch(pStr) {
                    case "rr r":
                        return cornerRooms[1];

                    case "rr -":
                        return capRooms[3];

                    case "r- r":
                        return capRooms[0];
                }
                break;

            //  Part - BL Corner
            case 3:
                switch(pStr) {
                    case "rr r":
                        return cornerRooms[2];

                    case "rr -":
                        return capRooms[2];

                    case "r- r":
                        return capRooms[1];
                }
                break;

            //  Part - BR Corner
            case 4:
                switch(pStr) {
                    case "rr r":
                        return cornerRooms[3];

                    case "rr -":
                        return capRooms[3];

                    case "r- r":
                        return capRooms[1];
                }
                break;
        }

        return null;
    }

    private GameObject CR_Edge(int pType, string pStr) {
        switch(pType) {
            //  Part - T Edge
            case 1:
                switch(pStr) {
                    //  SubPart - BLR Junction
                    case "rrr r":
                        return junctRooms[1];

                    //  SubPart - Horizontal Pillar
                    case "rrr -":
                        return pillarRooms[0];

                    //  SubPart - TL Corner
                    case "-rr r":
                        return cornerRooms[0];

                    //  SubPart - TR Corner
                    case "rr- r":
                        return cornerRooms[1];

                    //  SubPart - T Cap
                    case "-r- r":
                        return capRooms[0];

                    //  SubPart - L Cap
                    case "-rr -":
                        return capRooms[2];

                    //  SubPart - R Cap
                    case "rr- -":
                        return capRooms[3];
                }
                break;

            //  Part - B Edge
            case 2:
                switch(pStr) {
                    //  SubPart - TLR Junction
                    case "rrr r":
                        return junctRooms[0];

                    //  SubPart - Horizontal Pillar
                    case "rrr -":
                        return pillarRooms[0];

                    //  SubPart - BL Corner
                    case "-rr r":
                        return cornerRooms[2];

                    //  SubPart - BR Corner
                    case "rr- r":
                        return cornerRooms[3];

                    //  SubPart - B Cap
                    case "-r- r":
                        return capRooms[1];

                    //  SubPart - L Cap
                    case "-rr -":
                        return capRooms[2];

                    //  SubPart - R Cap
                    case "rr- -":
                        return capRooms[3];
                }
                break;

            //  Part - L Edge
            case 3:
                switch(pStr) {
                    //  SubPart - RTB Junction
                    case "rrr r":
                        return junctRooms[3];

                    //  SubPart - Vertical Pillar
                    case "rrr -":
                        return pillarRooms[1];

                    //  SubPart - TL Corner
                    case "-rr r":
                        return cornerRooms[0];

                    //  SubPart - BL Corner
                    case "rr- r":
                        return cornerRooms[2];

                    //  SubPart - L Cap
                    case "-r- r":
                        return capRooms[2];

                    //  SubPart - T Cap
                    case "-rr -":
                        return capRooms[0];

                    //  SubPart - B Cap
                    case "rr- -":
                        return capRooms[1];
                }
                break;

            //  Part - R Edge
            case 4:
                switch(pStr) {
                    //  SubPart - LTB Junction
                    case "rrr r":
                        return junctRooms[2];

                    //  SubPart - Vertical Pillar
                    case "rrr -":
                        return pillarRooms[1];

                    //  SubPart - TR Corner
                    case "-rr r":
                        return cornerRooms[1];

                    //  SubPart - BR Corner
                    case "rr- r":
                        return cornerRooms[3];

                    //  SubPart - R Cap
                    case "-r- r":
                        return capRooms[3];

                    //  SubPart - T Cap
                    case "-rr -":
                        return capRooms[0];

                    //  SubPart - B Cap
                    case "rr- -":
                        return capRooms[1];
                }
                break;
        }
        
        return null;
    }

    private GameObject CR_Center(string pStr) {
        switch(pStr) {
            //  SubPart - Center
            case "r rrr r":
                return centerRoom;

            //  SubPart - Horizontal Pillar
            case "- rrr -":
                return pillarRooms[0];

            //  SubPart - Vertical Pillar
            case "r -r- r":
                return pillarRooms[1];

            //  SubPart - TL Corner
            case "- -rr r":
                return cornerRooms[0];

            //  SubPart - TR Corner
            case "- rr- r":
                return cornerRooms[1];

            //  SubPart - BL Corner
            case "r -rr -":
                return cornerRooms[2];

            //  SubPart - BR Corner
            case "r rr- -":
                return cornerRooms[3];

            //  SubPart - TLR Junction
            case "r rrr -":
                return junctRooms[0];

            //  SubPart - BLR Junction
            case "- rrr r":
                return junctRooms[1];

            //  SubPart - LTB Junction
            case "r rr- r":
                return junctRooms[2];

            //  SubPart - RTB Junction
            case "r -rr r":
                return junctRooms[3];

            //  SubPart - T Cap
            case "- -r- r":
                return capRooms[0];

            //  SubPart - B Cap
            case "r -r- -":
                return capRooms[1];

            //  SubPart - L Cap
            case "- -rr -":
                return capRooms[2];

            //  SubPart - R Cap
            case "- rr- -":
                return capRooms[3];
        }
        
        return null;
    }
}