using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceBricks : MonoBehaviour {

    public GameObject brickPrefab;
    public TextMeshProUGUI helpText;
    public GameObject pathBlockerHolder;

    private Ray ray;
    private RaycastHit hit;
    private LayerMask mask = 1 << 10;
    private GameObject prevTile;
    private Color prevColor;
    private GameControl GC;
    private Node[,] tempGraph;
    private List<Node> StartToExitPath;
    private int bricks;

    void Start()
    {
        GC = FindObjectOfType<GameControl>();
        bricks = (GC.floorX + GC.floorY); // * 2;
        helpText.text = "Place " + bricks.ToString() + " more path blockers.";
    }

    void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (bricks > 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                tileData td = hit.collider.gameObject.GetComponent<tileData>();

                if (td != null && td.isWalkable && td.isPlaceable)
                {
                    if (prevTile != null)
                    {
                        prevTile.GetComponent<Renderer>().material.color = prevColor;
                    }
                    prevTile = hit.collider.gameObject;
                    prevColor = prevTile.GetComponent<Renderer>().material.color;

                    // create new graph with this tile marked as unwalkable,
                    // then see if we still have a valid path
                    prevTile.GetComponent<tileData>().isWalkable = false;
                    GC.GeneratePathfindingGraph(out tempGraph);
                    StartToExitPath = GC.GeneratePathFromTo(tempGraph, GC.startPos, GC.exitPos);
                    if (StartToExitPath != null)
                    {
                        prevTile.GetComponent<Renderer>().material.color = Color.green;
                    }
                    else
                    {
                        prevTile.GetComponent<Renderer>().material.color = Color.red;
                    }
                    prevTile.GetComponent<tileData>().isWalkable = true;

                    // Add path blocker
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (prevTile.GetComponent<Renderer>().material.color == Color.green)
                        {
                            Instantiate(brickPrefab, GC.floorTiles[td.tileX, td.tileY].transform.position, Quaternion.identity, pathBlockerHolder.transform);

                            prevTile.GetComponent<tileData>().isWalkable = false;
                            prevTile.GetComponent<Renderer>().material = GC.Barrier;
                            GC.PathFinder(true, true, true);
                            bricks--;
                            helpText.text = "Place " + bricks.ToString() + " more path blockers.";
                        }
                    }
                }
            }
        }
        else
        {
            GC.GameState++;
            this.enabled = false;
        }
    }
}
