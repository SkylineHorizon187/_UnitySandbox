using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDefense : MonoBehaviour {

    public static PlaceDefense PD;
    public GameObject arrowPrefab;
    public GameObject[] buildingPrefabs;

    private Ray ray;
    private RaycastHit hit;
    private LayerMask mask = 1 << 12;
    private GameObject arrow;
    private Transform lastClicked;

    void Start () {
        PD = this;
        arrow = Instantiate(arrowPrefab);
        arrow.SetActive(false);
	}
	
	void Update () {
        if (Menu.buildMenu.isOpen) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            if (hit.collider.tag == "PathBlocker" && hit.collider.transform.childCount < 1)
            {
                arrow.transform.position = hit.collider.gameObject.transform.position;
                arrow.SetActive(true);

                // check for click to display build menu
                if (Input.GetMouseButtonDown(0))
                {
                    lastClicked = hit.collider.gameObject.transform.parent;
                    Menu.buildMenu.MenuOpen(Input.mousePosition);
                }
            }
            else
            {
                arrow.SetActive(false);
            }
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    public void UpgradeTo(int building)
    {
        Instantiate(buildingPrefabs[building - 1], lastClicked.position, Quaternion.identity, lastClicked);
    }
}
