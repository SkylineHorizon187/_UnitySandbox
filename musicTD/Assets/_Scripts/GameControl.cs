using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
    [Range(1,24)]
    public int floorX, floorY;
    public GameObject floorPrefab, pathGO;
    public Material Barrier, Grass, Enter, Exit;
    public List<Node> StartToExitPath;
    public GameObject WayPointPrefab, PathBlockerPrefab;
    public int GameState = 0;
    public Image helpIcon;
    public TextMeshProUGUI helpText;
    public Sprite flagSprite, brickSprite, defenseSprite;
    public GameObject floorTileHolder;

    public GameObject[,] floorTiles;
    private GameObject prevTile, startTile, exitTile, prevStartTile, prevExitTile;
    private Color prevColor;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask mask;
    public Vector2Int startPos, exitPos;
    private Node[,] mainGraph, tempGraph;
    public GameObject WP1, WP2;
    private int prevGameState = -1;
    
    public Vector3 SpawnPosition()
    {
        return floorTiles[startPos.x, startPos.y].transform.position;
    }
    public Vector3 TilePosition(int x, int y)
    {
        return floorTiles[x, y].transform.position;
    }

    void Start () {
        // initialize
        floorTiles = new GameObject[(floorX+2), (floorY+2)];
        startPos = Vector2Int.zero;
        exitPos = Vector2Int.zero;
        mask = 1 << 10;
        Random.InitState(Music.NameSeed);

        // build map
        for (int i = 0; i < floorX; i++)
        {
            for (int j = 0; j < floorY; j++)
            {
                floorTiles[i,j] = Instantiate(floorPrefab, new Vector3(-floorX / 2 + i, 0, -floorY / 2 + j), Quaternion.Euler(90, 0, 0), floorTileHolder.transform);
                tileData td = floorTiles[i, j].GetComponent<tileData>();

                td.tileX = i;
                td.tileY = j;
                if (i == 0 || i == floorX - 1 || j == 0 || j == floorY - 1)
                {
                    floorTiles[i, j].GetComponent<Renderer>().material = Barrier;
                    td.isWalkable = false;
                    td.isPlaceable = false;
                }
                else
                {
                    floorTiles[i, j].GetComponent<Renderer>().material = Grass;
                    td.isWalkable = true;
                    td.isPlaceable = true;
                }
            }
        }
        RandomizeStartExit();
        // pathfinding
        PathFinder(true, true, true);
    }

    void Update () {
        if (GameState != prevGameState)
        {
            switch (GameState)
            {
                case 0:
                    helpIcon.sprite = flagSprite;
                    helpText.text = "Place 2 waypoints for monsters to visit before exiting.";
                    GetComponent<PlaceFlag>().enabled = true;
                    break;
                case 1:
                    helpIcon.sprite = brickSprite;
                    helpText.text = "Place pathing bricks.";
                    GetComponent<PlaceBricks>().enabled = true;
                    break;
                case 2:
                    helpIcon.sprite = defenseSprite;
                    helpText.text = "Build defenses!  Click on a pathing brick to access the build menu.";
                    GetComponent<PlaceDefense>().enabled = true;
                    break;
                default:
                    break;
            }

            prevGameState = GameState;
        }
	}

    public void RandomizeStartExit()
    {
        // reset any existing start/exit tiles
        if (prevStartTile != null)
        {
            prevStartTile.GetComponent<Renderer>().material = Barrier;
            prevStartTile.GetComponent<tileData>().isWalkable = false;
        }
        if (prevExitTile != null)
        {
            prevExitTile.GetComponent<Renderer>().material = Barrier;
            prevExitTile.GetComponent<tileData>().isWalkable = false;
        }

        // Start Tile
        if (Random.value < .5f)
        {
            startPos.x = 0;
            startPos.y = Random.Range(2, floorY-2);
        }
        else
        {
            startPos.x = Random.Range(2, floorX - 2);
            startPos.y = 0;
        }
        floorTiles[startPos.x, startPos.y].GetComponent<Renderer>().material = Enter;
        floorTiles[startPos.x, startPos.y].GetComponent<tileData>().isWalkable = true;
        prevStartTile = floorTiles[startPos.x, startPos.y];

        // Exit Tile
        if (Random.value < .5f)
        {
            exitPos.x = floorX - 1;
            exitPos.y = Random.Range(2, floorY - 2);
        }
        else
        {
            exitPos.x = Random.Range(2, floorX - 2);
            exitPos.y = floorY - 1;
        }
        floorTiles[exitPos.x, exitPos.y].GetComponent<Renderer>().material = Exit;
        floorTiles[exitPos.x, exitPos.y].GetComponent<tileData>().isWalkable = true;
        prevExitTile = floorTiles[exitPos.x, exitPos.y];

        PathFinder(true, true, true);
    }

    public void PathFinder(bool GenerateGraph, bool GeneratePath, bool ShowPath)
    {
        if (GenerateGraph)
        {
            GeneratePathfindingGraph(out mainGraph);
        }

        if (GeneratePath)
        {
            StartToExitPath = GeneratePathFromTo(mainGraph, startPos, exitPos);
        }

        if (ShowPath && StartToExitPath != null)
        {
            VolumetricLines.VolumetricMultiLineBehavior lr = pathGO.GetComponent<VolumetricLines.VolumetricMultiLineBehavior>();
            Vector3[] points = new Vector3[StartToExitPath.Count];
            for (int i = 0; i < StartToExitPath.Count; i++)
            {
                points[i] = floorTiles[StartToExitPath[i].x, StartToExitPath[i].y].transform.position;
                points[i].y += .1f;
            }
            lr.UpdateLineVertices(points);
            pathGO.SetActive(true);
        }
        else
        {
            pathGO.SetActive(false);
        }
    }

    public void GeneratePathfindingGraph(out Node[,] graph)
    {
        // Initialize the array
        graph = new Node[floorX, floorY];

        // Initialize a Node for each spot in the array
        for (int x = 0; x < floorX; x++)
        {
            for (int y = 0; y < floorY; y++)
            {
                graph[x, y] = new Node
                {
                    x = x,
                    y = y
                };
            }
        }

        // Now that all the nodes exist, calculate their neighbours
        for (int x = 0; x < floorX; x++)
        {
            for (int y = 0; y < floorY; y++)
            {
                // This is the 4-way connection version:
                if(x > 0 && UnitCanEnterTile(x-1, y))  
                    graph[x,y].neighbours.Add( graph[x-1, y] );
                if(x < floorX-1 && UnitCanEnterTile(x+1, y))
                    graph[x,y].neighbours.Add( graph[x+1, y] );
                if(y > 0 && UnitCanEnterTile(x, y-1))
                    graph[x,y].neighbours.Add( graph[x, y-1] );
                if(y < floorY-1 && UnitCanEnterTile(x, y+1))
                    graph[x,y].neighbours.Add( graph[x, y+1] );
            }
        }
    }

    public bool UnitCanEnterTile(int x, int y)
    {
        return floorTiles[x,y].GetComponent<tileData>().isWalkable;
    }

    public List<Node> GeneratePathFromTo(Node[,] graph, Vector2Int from, Vector2Int to)
    {
        List<Node> allPath = new List<Node>();
        List<Node> curPath = new List<Node>();

        if (WP1 != null)
        {
            WayPoint wp1 = WP1.GetComponent<WayPoint>();
            curPath = GenPath(graph, from, new Vector2Int(wp1.boardX, wp1.boardY));
            if (curPath != null)
            {
                allPath.AddRange(curPath);

                if (WP2 != null)
                {
                    WayPoint wp2 = WP2.GetComponent<WayPoint>();
                    curPath = GenPath(graph, new Vector2Int(wp1.boardX, wp1.boardY), new Vector2Int(wp2.boardX, wp2.boardY));
                    if (curPath != null)
                    {
                        allPath.AddRange(curPath);
                        curPath = GenPath(graph, new Vector2Int(wp2.boardX, wp2.boardY), to);
                        if (curPath != null)
                        {
                            allPath.AddRange(curPath);
                        } else
                        {
                            allPath = null;
                        }
                    }
                    else
                    {
                        allPath = null;
                    }
                } else
                {
                    curPath = GenPath(graph, new Vector2Int(wp1.boardX, wp1.boardY), to);
                    if (curPath != null)
                    {
                        allPath.AddRange(curPath);
                    } else
                    {
                        allPath = null;
                    }
                }
            } else
            {
                allPath = null;
            }
        } else
        {
            allPath = GenPath(graph, from, to);
        }

        return allPath;
    }

    public List<Node> GenPath(Node[,] graph, Vector2Int from, Vector2Int to)
    {
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = graph[from.x, from.y];
        Node target = graph[to.x, to.y];
        dist[source] = 0;
        prev[source] = null;

        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value
        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;  // Found the target, exit the while loop!
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + 1;
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        // If we get here, we either found the shortest route
        // to our target, or there is no route at ALL to our target.
        if (prev[target] == null)
        {
            // No route between our target and the source
            return null;
        }

        List<Node> currentPath = new List<Node>();
        Node curr = target;

        // Step through the "prev" chain and add it to our path
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        // Right now, currentPath describes a route from our target to our source
        // So we need to invert it!
        currentPath.Reverse();
        return currentPath;
    }
}
