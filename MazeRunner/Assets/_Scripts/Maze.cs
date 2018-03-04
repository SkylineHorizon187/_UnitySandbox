using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    // Custom Room Class
    [System.Serializable]
    public class Room
    {
        public bool visited;
        public int front;
        public int back;
        public int left;
        public int right;
        public int top;
        public int bottom;
        public int yFloor;
        public int xPos;
        public int zPos;

        public Room(int f, int x, int y)
        {
            visited = false;
            front  = 1;
            back   = 1;
            left   = 1;
            right  = 1;
            top    = 1;
            bottom = 1;
            yFloor = f;
            xPos = x;
            zPos = y;
        }
    }

	public static Maze instance;
    // Maze Dimensions and Room Tiles
    public int sizeX = 5;
    public int sizeZ = 5;
    public int yFloors = 1;
	public GameObject roomPrefab;
	public GameObject wallPrefab;
    [HideInInspector]
    public int wallToBreak;
	[HideInInspector]
	public Room[,,] rooms;
	[HideInInspector]
	public int MS;

    // Private vars
    private List<Room> lastRoom;
    private int curF;
    private int curX;
    private int curY;

	void Awake() {
		instance = this;
		MS = 4;
	}
    
	void Start() {
		newMaze ();
	}

    public void newMaze () {
		// Remove any existing maze pieces
		int children = transform.childCount;
		for (int i = children-1; i >= 0; i--) {
			Destroy (transform.GetChild (i).gameObject);
		}
		// Create new maze
        rooms = new Room[yFloors, sizeX, sizeZ];
        initializeRooms();
        createMaze();
        drawMaze();
	}

    // Create the Maze
    void createMaze()
    {
        int totalRooms = sizeX * sizeZ * yFloors;
        int visitedRooms = 1;
        wallToBreak = 0;
        Room currentRoom = rooms[Random.Range(0, yFloors), Random.Range(0, sizeX), Random.Range(0, sizeZ)];
        Room neighbor = new Room(0,0,0);
        lastRoom = new List<Room>();

        currentRoom.visited = true;
        while (visitedRooms < totalRooms)
        {
            neighbor = GetNeighbor(currentRoom);
            if (neighbor.visited == false)
            {
                lastRoom.Add(currentRoom);
                neighbor.visited = true;
                BreakWall(currentRoom, neighbor);
                visitedRooms++;
            }
            currentRoom = neighbor;
        }
    }

    private void BreakWall(Room cur, Room neg)
    {
        switch (wallToBreak)
        {
            case 1:    // North
                cur.front = 0;
                neg.back = 0;
                break;
            case 2:    // South
                cur.back = 0;
                neg.front = 0;
                break;
            case 3:    // West
                cur.left = 0;
                neg.right = 0;
                break;
            case 4:    // East
                cur.right = 0;
                neg.left = 0;
                break;
            case 5:    // Up
                cur.top = 0;
                neg.bottom = 0;
                break;
            case 6:    // Down
                cur.bottom = 0;
                neg.top = 0;
                break;
        }
    }

    private Room GetNeighbor(Room c)
    {
        List<Room> neighbors = new List<Room>();
        int[] breakWall = new int[6];

        // Check for Back neighbor
        if (c.zPos < sizeZ - 1 && rooms[c.yFloor, c.xPos, c.zPos + 1].visited == false)
        {
            neighbors.Add(rooms[c.yFloor, c.xPos, c.zPos + 1]);
            breakWall[neighbors.Count - 1] = 1;
        }
        // Check for Front neighbor
        if (c.zPos > 0 && rooms[c.yFloor, c.xPos, c.zPos - 1].visited == false)
        {
            neighbors.Add(rooms[c.yFloor, c.xPos, c.zPos - 1]);
            breakWall[neighbors.Count - 1] = 2;
        }
        // Check for West neighbor
        if (c.xPos > 0 && rooms[c.yFloor, c.xPos - 1, c.zPos].visited == false)
        {
            neighbors.Add(rooms[c.yFloor, c.xPos - 1, c.zPos]);
            breakWall[neighbors.Count - 1] = 3;
        }
        // Check for East neighbor
        if (c.xPos < sizeX-1 && rooms[c.yFloor, c.xPos + 1, c.zPos].visited == false)
        {
            neighbors.Add(rooms[c.yFloor, c.xPos + 1, c.zPos]);
            breakWall[neighbors.Count - 1] = 4;
        }
        // Check for Up neighbor
        if (c.yFloor < yFloors-1 && rooms[c.yFloor + 1, c.xPos, c.zPos].visited == false)
        {
            neighbors.Add(rooms[c.yFloor + 1, c.xPos, c.zPos]);
            breakWall[neighbors.Count - 1] = 5;
        }
        // Check for Down neighbor
        if (c.yFloor > 0 && rooms[c.yFloor - 1, c.xPos, c.zPos].visited == false)
        {
            neighbors.Add(rooms[c.yFloor - 1, c.xPos, c.zPos]);
            breakWall[neighbors.Count - 1] = 6;
        }

        if (neighbors.Count > 0)  // we found valid neighbors, return a random one
        {
            int ret = Random.Range(0, neighbors.Count - 1);
            wallToBreak = breakWall[ret];
            return neighbors[ret];
        }
        else  // no valid neighbors, backup to previous room
        {
            lastRoom.RemoveAt(lastRoom.Count - 1);
            return lastRoom[lastRoom.Count - 1];
        }
    }

    // Draw the Maze
    void drawMaze()
    {
	    for (int f = 0; f < yFloors; f++) {
	        for (int x = 0; x < sizeX; x++) {
	            for (int y = 0; y < sizeZ; y++) {
	                Room r = rooms[f, x, y];
					GameObject room = Instantiate (roomPrefab, new Vector3(r.xPos * MS, r.yFloor * MS, r.zPos * MS), Quaternion.identity, transform);
					room.GetComponent<Motel6> ().myFloor = f;


					if (r.front == 1) {
						MakeWall (room.transform, 5);
	                }
					if (r.back == 1) {
						MakeWall (room.transform, 4);
	                }
	                if (r.left == 1) {
						MakeWall (room.transform, 2);
	                }
	                if (r.right == 1) {
						MakeWall (room.transform, 3);
	                }
	                if (r.top == 1) {
						MakeWall (room.transform, 0);
	                }
	                if (r.bottom == 1) {
						MakeWall (room.transform, 1);
	                }
	            }
	        }
	    }
    }

	void MakeWall(Transform parent, int child) {
		GameObject wall = Instantiate (wallPrefab);
		wall.transform.SetParent (parent.GetChild (child));
		wall.transform.localPosition = Vector3.zero;
		wall.transform.localRotation = Quaternion.identity;
		wall.transform.localScale = Vector3.one;
	}

    // Initialize all the Maze Rooms
    void initializeRooms()
    {
        for (int f = 0; f < yFloors; f++) {
            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeZ; y++) {
                    rooms[f, x, y] = new Maze.Room(f, x, y);
                }
            }
        }
    }
	
}
