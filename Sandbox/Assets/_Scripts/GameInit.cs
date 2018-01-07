using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour {

    public GameObject PlayerPrefab;

    public GameObject[] RedSpawnPoints;
    public GameObject[] BlueSpawnPoints;


	// Use this for initialization
	void Start () {
        Instantiate(PlayerPrefab, RedSpawnPoints[Random.Range(0, RedSpawnPoints.Length)].transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
