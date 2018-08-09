using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceFlag : MonoBehaviour {

    public GameObject flagPrefab;

    private Ray ray;
    private RaycastHit hit;
    private LayerMask mask = 1 << 10;
    private GameObject prevTile;
    private Color prevColor;
    private GameControl GC;
    private Node[,] tempGraph;
    private List<Node> StartToExitPath;
    
    void Start () {
        GC = FindObjectOfType<GameControl>();	
	}
	
	void Update () {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (GC.WP1 == null || GC.WP2 == null)
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
                        prevTile.GetComponent<Renderer>().material.color = Color.gray;
                    }
                    else
                    {
                        prevTile.GetComponent<Renderer>().material.color = Color.red;
                    }
                    prevTile.GetComponent<tileData>().isWalkable = true;

                    // Add waypoint
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (prevTile.GetComponent<Renderer>().material.color == Color.gray)
                        {
                            if (GC.WP1 == null)
                            {
                                GC.WP1 = Instantiate(flagPrefab, prevTile.transform.position, Quaternion.identity);
                                GC.WP1.transform.GetChild(0).GetComponent<TextMeshPro>().text = "1";

                                WayPoint wp = GC.WP1.GetComponent<WayPoint>();
                                tileData ntd = prevTile.GetComponent<tileData>();
                                wp.boardX = ntd.tileX;
                                wp.boardY = ntd.tileY;

                            }
                            else if (GC.WP2 == null)
                            {
                                GC.WP2 = Instantiate(flagPrefab, prevTile.transform.position, Quaternion.identity);
                                GC.WP2.transform.GetChild(0).GetComponent<TextMeshPro>().text = "2";

                                WayPoint wp = GC.WP2.GetComponent<WayPoint>();
                                tileData ntd = prevTile.GetComponent<tileData>();
                                wp.boardX = ntd.tileX;
                                wp.boardY = ntd.tileY;
                            }

                            prevTile.GetComponent<tileData>().isPlaceable = false;
                            prevTile.GetComponent<Renderer>().material.color = prevColor;
                            GC.PathFinder(true, true, true);
                        }
                    }
                }
            }
        } else
        {
            GC.GameState++;
            this.enabled = false;
        }
    }
}
