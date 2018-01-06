using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickSpawn : MonoBehaviour {

	public static BrickSpawn BSpawn;
	public GameObject brickPrefab;
	public float spawnTime;
	public int brickHealth;
	public float moveSpeed;
	public Text scoreText;

	private float timer = 0f;
	private GameObject brick;
	private BrickScript BS;
	private int gameScore;

	void Start () {
		BSpawn = this;
		InvokeRepeating ("IncreaseBrickHealth", 20f, 20f);
		InvokeRepeating ("IncreaseBrickMovementSpeed", 30f, 30f);
		gameScore = 0;
	}

	void IncreaseBrickHealth() {
		brickHealth += 2;
	}

	void IncreaseBrickMovementSpeed() {
		moveSpeed += .3f;
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer >= spawnTime) {
			timer -= spawnTime;

			int RandNSWE = Random.Range (0, 4);
			if (RandNSWE == 0) {			// North
				SpawnBrick (new Vector3(Random.Range (-12f, 12f), 10.5f, 1), Quaternion.Euler (0,0,0));
			} else if (RandNSWE == 1) {		// South
				SpawnBrick (new Vector3(Random.Range (-12f, 12f), -10.5f, 1), Quaternion.Euler (0,0,180));
			} else if (RandNSWE == 2) {		// West
				SpawnBrick (new Vector3(-17.25f, Random.Range (-8f, 8f), 1), Quaternion.Euler (0,0,90));
			} else if (RandNSWE == 3) {		// East
				SpawnBrick (new Vector3(17.25f, Random.Range (-8f, 8f), 1), Quaternion.Euler (0,0,-90));
			}
		}
	}

	void SpawnBrick(Vector3 pos, Quaternion rot) {
		brick = Instantiate (brickPrefab, pos, rot, transform);
		BS = brick.GetComponent<BrickScript> ();
		BS.brickHealth = brickHealth;
		BS.moveSpeed = moveSpeed;
	}

	public void Score(int val) {
		gameScore += val;
		scoreText.text = "Score: " + gameScore;
	}
}
