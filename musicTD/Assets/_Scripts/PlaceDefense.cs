using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDefense : MonoBehaviour {

    public static PlaceDefense PD;
    public GameObject[] buildingPrefabs;
    public GameObject pathBlockerHolder;
    public GameObject selectedBrick;

    void Start () {
        PD = this;
        selectedBrick = new GameObject();
    }
	
    public void UpgradeTo(int building)
    {
        if (selectedBrick.transform.childCount > 1)
        {
            // Destroy the old building that was here
            Destroy(selectedBrick.transform.GetChild(1).gameObject);

        }
        // Place new building (if needed)
        if (building > -1)
        {
            Instantiate(buildingPrefabs[building - 1], selectedBrick.transform.position, Quaternion.identity, selectedBrick.transform);
        }
    }
}
