using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

	public struct Cloud {
		public GameObject c;
		public int xDir;
		public int speed;
	}
	public GameObject[] CloudPrefabs;
	public int CloudMinHeight;
	public int CloudMaxHeight;
	public int CloudMinSpawnRate;
	public int CloudMaxSpawnRate;

	[Range(-1,1)] public int windDirection;
	[Range(2,25)] public int windSpeed;

	private List<Cloud> clouds;
	private float timer;

	void Start () {
		clouds = new List<Cloud> ();
		timer = 1;
		Invoke ("RandomizeWeather", .5f);
	}

	void RandomizeWeather() {
		if (AllTerrain.AT.pRandom.Next (1) == 1) {
			windDirection = 1;
		} else {
			windDirection = -1;
		}
		windSpeed = AllTerrain.AT.pRandom.Next (2, 25);
	}
	
	void Update () {
		timer -= Time.deltaTime;

		if (timer <= 0) {
			Cloud C = new Cloud ();
			C.xDir = windDirection;
			C.speed = windSpeed * 2;
			if (C.xDir == 1) {
				C.c = Instantiate (CloudPrefabs [Random.Range (0, CloudPrefabs.Length)], new Vector3 (-70f, Random.Range (CloudMinHeight, CloudMaxHeight), 1), Quaternion.identity);
			} else {
				C.c = Instantiate (CloudPrefabs [Random.Range (0, CloudPrefabs.Length)], new Vector3 (570f, Random.Range (CloudMinHeight, CloudMaxHeight), 1), Quaternion.identity);
			}
			clouds.Add (C);
			timer = Random.Range (CloudMinSpawnRate, CloudMaxSpawnRate);
		}

		for (int i = 0; i < clouds.Count; i++) {
			clouds[i].c.transform.Translate (new Vector3 (clouds[i].xDir, 0f, 0f) * clouds[i].speed * Time.deltaTime);
			if (clouds[i].c.transform.position.x < -70f || clouds[i].c.transform.position.x > 570f) {
				Destroy (clouds[i].c);
				clouds.Remove (clouds[i]);
				i--;
			}
		}
	}
}
