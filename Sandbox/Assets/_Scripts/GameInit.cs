using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour {

    public GameObject PlayerPrefab;

    public GameObject[] RedSpawnPoints;
    public GameObject[] BlueSpawnPoints;


	// Use this for initialization
	void Start () {
        CreatePlayer(1);
        CreatePlayer(2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    GameObject CreatePlayer(int team) {
        GameObject Player;
        Player = Instantiate(PlayerPrefab);
        

        if(team == 1) {
            Player.transform.position = RedSpawnPoints[Random.Range(0, RedSpawnPoints.Length)].transform.position;
            Player.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (team == 2) {
            Player.transform.position = BlueSpawnPoints[Random.Range(0, BlueSpawnPoints.Length)].transform.position;
            Player.GetComponent<SpriteRenderer>().color = Color.blue;
            
        }

        // Clean this up later to be handled in _Tags
        Player.GetComponent<PlayerControls>().Team = team;
        Player.GetComponent<_Tags>().AddTag("Team" + team);

        return Player;
    }


}
