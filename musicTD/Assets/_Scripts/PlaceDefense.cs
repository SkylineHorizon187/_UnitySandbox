using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDefense : MonoBehaviour {

    public static PlaceDefense PD;
    public GameObject arrowPrefab;
    public GameObject[] buildingPrefabs;
    public GameObject pathBlockerHolder;

    private Ray ray;
    private RaycastHit hit;
    private LayerMask mask = 1 << 12;
    private GameObject arrow, lastSelected, lastClicked;
    private List<GameObject> bricks, children;
    private int brickID;

    void Start () {
        PD = this;
        arrow = Instantiate(arrowPrefab);
        arrow.SetActive(false);

        bricks = new List<GameObject>();
        children = new List<GameObject>();
        for (int i = 0; i < pathBlockerHolder.transform.childCount; i++)
        {
            bricks.Add(pathBlockerHolder.transform.GetChild(i).gameObject);
            children.Add(null);
        }
    }
	
	void Update () {
        if (Menu.buildMenu.isOpen) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            if (hit.collider.tag == "PathBlocker")
            {
                if (hit.collider.gameObject.transform.parent.gameObject != lastSelected)
                {
                    UnHilite(lastSelected);
                    lastSelected = null;

                    for (brickID = 0; brickID < bricks.Count; brickID++)
                    {
                        if (bricks[brickID] == hit.collider.gameObject.transform.parent.gameObject)
                            break;
                    }

                    if (brickID < bricks.Count)
                    { 
                        lastSelected = bricks[brickID];
                        Hilite(lastSelected);
                    }
                }

                // check for click to display build menu
                if (lastSelected != null && Input.GetMouseButtonDown(0))
                {
                    lastClicked = lastSelected;
                    Menu.buildMenu.MenuOpen(Input.mousePosition);
                }
            }
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    public void UpgradeTo(int building)
    {
        lastClicked.GetComponent<blockChild>().child = Instantiate(buildingPrefabs[building - 1], lastClicked.position, Quaternion.identity, lastClicked);
    }
}
