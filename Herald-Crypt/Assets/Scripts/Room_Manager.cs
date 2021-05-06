using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviour {
    public int ringAmount;
    public int roomAmount;

    public List<GameObject> capRooms;       //0-TC, 1-BC, 2-LC, 3-RC
    public List<GameObject> cornerRooms;    //0-TL, 1-TR, 2-BL, 3-BR
    public List<GameObject> edgeRooms;      //0-TE, 1-BE, 2-LE, 3-RE
    public List<GameObject> junctRooms;     //0-TLR, 1-BLR, 2-LTB, 3-RTB
    public List<GameObject> pillarRooms;    //0-HP, 1-VP

    public GameObject startRoom;
    public GameObject centerRoom;

    [HideInInspector]
    public Vector2 tileSize;// = new Vector2(0.5f, 0.5f);
    [HideInInspector]
    public Vector2 roomSize;// = new Vector2(5.5f, 5.5f);

    public GameObject player;
    private string[,] stringGrid;
    private GameObject[,] roomGrid;

    [HideInInspector]
    public int roomWidth;
    [HideInInspector]
    public int roomHeight;

    // Stats for generating grids for pathfinding
    [HideInInspector]
    public Vector2 lowest = new Vector2(999, 999);
    [HideInInspector]
    public Vector2 highest = new Vector2 (-999, -999);
    [HideInInspector]
    public List<GameObject> roomList;

    void Start() {
        //CreateStringGrid();

        roomList = new List<GameObject>();

        // Initialize roomSize based on tileSize
        tileSize = new Vector2(0.5f, 0.5f);
        roomSize = tileSize * 11;
    }

    public void CreateStringGrid() {
        //  Part - Create Ring Amount (Odd > 4)
        if (ringAmount < 5) {
            ringAmount = 5;
        }

        if (ringAmount % 2 == 0) {
            ringAmount++;
        }

        //  Part - Create Room Amount (> 7)
        if (roomAmount < 7){
            roomAmount = 7;
        }

        stringGrid = new string[2 * ringAmount + 1, 2 * ringAmount + 1];
        int midpoint = stringGrid.GetLength(0) / 2;

        //  Part - Setup Base String Grid
        for (int a = 0; a < stringGrid.GetLength(0); a++) {
            for (int b = 0; b < stringGrid.GetLength(1); b++) {
                stringGrid[a, b] = "-";
            }
        }

        //  Part - Create String Grid
        for (int i = 0; i < ringAmount; i++) {
            if (roomAmount > 0) {
                //  Part - Starting 3x3
                if (i == 0) {
                    stringGrid[midpoint, midpoint] = "r";

                    stringGrid[midpoint - 1, midpoint] = "r";
                    stringGrid[midpoint + 1, midpoint] = "r";
                    stringGrid[midpoint, midpoint - 1] = "r";
                    stringGrid[midpoint, midpoint + 1] = "r";

                    int rand1 = Random.Range(1, 2);
                    if (rand1 == 1) {
                        stringGrid[midpoint - 1, midpoint - 1] = "r";
                        stringGrid[midpoint + 1, midpoint + 1] = "r";
                    }
                    else if (rand1 == 2) {
                        stringGrid[midpoint + 1, midpoint - 1] = "r";
                        stringGrid[midpoint - 1, midpoint + 1] = "r";
                    }

                    roomAmount -= 7;
                }

                else {
                    for (int y = -i; y <= i; y++) {
                        for (int x = -i; x <= i; x++) {

                            if (stringGrid[midpoint + x, midpoint + y] == "r") {
                                //  Part - Top Row
                                if (y == -i) {
                                    //  Part - TL Corner
                                    if (x == -i) {
                                        CS_Corner(1, new Vector2(midpoint + x, midpoint + y));
                                    }

                                    //  Part - T Edge
                                    else if (x > -i && x < i) {
                                        CS_CEdge(1, new Vector2(midpoint + x, midpoint + y));
                                    }

                                    //  Part - TR Corner
                                    else if (x == i) {
                                        CS_Corner(2, new Vector2(midpoint + x, midpoint + y));
                                    }
                                }

                                //  Part - Middle Row
                                else if (y > -i && y < i) {
                                    //  Part - L Edge
                                    if (x == -i) {
                                        CS_CEdge(3, new Vector2(midpoint + x, midpoint + y));
                                    }

                                    //  Part - R Edge
                                    else if (x == i) {
                                        CS_CEdge(4, new Vector2(midpoint + x, midpoint + y));
                                    }
                                }

                                //  Part - Bottom Row
                                else if (y == i) {
                                    //  Part - BL Corner
                                    if (x == -i) {
                                        CS_Corner(3, new Vector2(midpoint + x, midpoint + y));
                                    }

                                    //  Part - B Edge
                                    else if (x > -i && x < i) {
                                        CS_CEdge(2, new Vector2(midpoint + x, midpoint + y));
                                    }

                                    //  Part - BR Corner
                                    else if (x == i) {
                                        CS_Corner(4, new Vector2(midpoint + x, midpoint + y));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        roomWidth = stringGrid.GetLength(0);
        roomHeight = stringGrid.GetLength(1);

        CreateRoomGrid(stringGrid, roomWidth, roomHeight);
    }

    private void CS_Corner(int pType, Vector2 pIndex) {
        int rand = 0;

        if (roomAmount >= 2) {
            rand = Random.Range(1, 4);
        }

        else if (roomAmount == 1) {
            rand = Random.Range(1, 2);
        }

        int pX = (int)pIndex.x;
        int pY = (int)pIndex.y;

        switch(pType) {
            //  Part - TL Corner
            case 1:
                switch(rand) {
                    case 1:
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 2:
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 3:
                        stringGrid[pX - 1, pY - 1]   = "r";
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;

                    case 4:
                        stringGrid[pX - 1, pY - 1]   = "r";
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                }
                break;

            //  Part - TR Corner
            case 2:
                switch(rand) {
                    case 1:
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 2:
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 3:
                        stringGrid[pX + 1, pY - 1]   = "r";
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;

                    case 4:
                        stringGrid[pX + 1, pY - 1]   = "r";
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                }
                break;

            //  Part - BL Corner
            case 3:
                switch(rand) {
                    case 1:
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 2:
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 3:
                        stringGrid[pX - 1, pY + 1]   = "r";
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;

                    case 4:
                        stringGrid[pX - 1, pY + 1]   = "r";
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                }
                break;

            //  Part - BR Corner
            case 4:
                switch(rand) {
                    case 1:
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 2:
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;

                    case 3:
                        stringGrid[pX + 1, pY + 1]   = "r";
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;

                    case 4:
                        stringGrid[pX + 1, pY + 1]   = "r";
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                }
                break;
        }
    }

    private void CS_CEdge(int pType, Vector2 pIndex) {
        int rand = 0;

        if (roomAmount >= 3) {
            rand = Random.Range(1, 4);
        }

        else if (roomAmount == 2) {
            rand = Random.Range(1, 3);
        }

        else if (roomAmount == 1) {
            rand = 1;
        }

        int pX = (int)pIndex.x;
        int pY = (int)pIndex.y;

        switch(pType) {
            //  Part - T Edge
            case 1:
                switch (rand) {
                    case 1:
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;
                    case 2:
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX + 1, pY - 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 3:
                        stringGrid[pX - 1, pY - 1]   = "r";
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 4:
                        stringGrid[pX - 1, pY - 1]   = "r";
                        stringGrid[pX, pY - 1]       = "r";
                        stringGrid[pX + 1, pY - 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 3;
                        break;
                }
                break;

            //  Part - B Edge
            case 2:
                switch (rand) {
                    case 1:
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;
                    case 2:
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX + 1, pY + 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 3:
                        stringGrid[pX - 1, pY + 1]   = "r";
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 4:
                        stringGrid[pX - 1, pY + 1]   = "r";
                        stringGrid[pX, pY + 1]       = "r";
                        stringGrid[pX + 1, pY + 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 3;
                        break;
                }
                break;

            //  Part - L Edge
            case 3:
                switch (rand) {
                    case 1:
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;
                    case 2:
                        stringGrid[pX - 1, pY - 1]   = "r";
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 3:
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX - 1, pY + 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 4:
                        stringGrid[pX - 1, pY - 1]   = "r";
                        stringGrid[pX - 1, pY]       = "r";
                        stringGrid[pX - 1, pY + 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 3;
                        break;
                }
                break;

            //  Part - R Edge
            case 4:
                switch (rand) {
                    case 1:
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 1;
                        break;
                    case 2:
                        stringGrid[pX + 1, pY - 1]   = "r";
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 3:
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX + 1, pY + 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 2;
                        break;
                    case 4:
                        stringGrid[pX + 1, pY - 1]   = "r";
                        stringGrid[pX + 1, pY]       = "r";
                        stringGrid[pX + 1, pY + 1]   = "r";
                        stringGrid[pX, pY]           = "r";
                        roomAmount -= 3;
                        break;
                }
                break;
        }
    }

    private void CreateRoomGrid(string [,] pStr, int pW, int pH) {
        // Part - Create String Grid, Setup Start Room, Move Player
        roomGrid = new GameObject[pW, pH];
       
        GameObject sRoom = Instantiate(startRoom, new Vector3(pW / 2 * roomSize[0], 0 - pH / 2 * roomSize[1], 0), Quaternion.identity);
        roomList.Add(sRoom);

        player.transform.position = new Vector3(pW / 2 * roomSize[0], 0 - pH / 2 * roomSize[1], 0);

        // Part - Setup Room Grid
        string roomStr = "";

        for (int y = 0; y < roomGrid.GetLength(1); y++) {
            for (int x = 0; x < roomGrid.GetLength(0); x++) {
                if (roomGrid[y, x] == null) {
                    //  Part - Top Row
                    if (y == 0) {
                        //  SubPart - TL Corner
                        if (x == 0) {
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
                            roomStr = pStr[x, y - 1] + " " + pStr[x - 1, y] + pStr[x, y] + pStr[x + 1, y] + " " + pStr[x, y + 1];
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
                        GameObject room = Instantiate(roomGrid[y, x], new Vector3(x * roomSize[0], 0 - y * roomSize[1], 0), Quaternion.identity);
                        roomList.Add(room);

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