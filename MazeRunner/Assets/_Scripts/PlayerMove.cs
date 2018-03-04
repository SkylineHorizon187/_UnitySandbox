using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {

	public static PlayerMove instance;
	public GameObject exitPortalPrefab;
	public GameObject portalKeysPrefab;
	public Image mazeExplored;
	public Image floorExplored;
	public Text floorNumber;
	public Text keyNumber;
	public Image nextLevel;
	public Text nextLevelText;

	public float sensitivityX;
	public float sensitivityY;
	public float moveSpeed;
	public int NumKeys = 0;
	[HideInInspector]
	public int keysfound = 0;

	float rotationX = 0F;
	float rotationY = 0F;
	Quaternion originalRotation;

	private int[] roomsVisited;
	private int roomsPerFloor;
	private int totalRooms;
	private GameObject exitPortal;
	private bool playerActive;

	void Awake() {
		instance = this;
	}

	void Start() {
		// get camera's starting rotation
		originalRotation = transform.localRotation;
		PlayerSetup ();
	}

	public void PlayerSetup () {
		playerActive = false;

		// set camera's starting rotation
		transform.localRotation = originalRotation;
		// lock the cursor
		Cursor.lockState = CursorLockMode.Locked;

		// tracking variables for progress bars
		roomsVisited = new int[Maze.instance.yFloors];
		roomsPerFloor = Maze.instance.sizeX * Maze.instance.sizeZ;
		totalRooms = roomsPerFloor * Maze.instance.yFloors;
		keysfound = 0;
		keyNumber.text = keysfound.ToString () + " of " + NumKeys.ToString ();

		// set initial location to random room on first floor
		transform.position = new Vector3(Random.Range (0, Maze.instance.sizeX) * Maze.instance.MS, 0, Random.Range (0, Maze.instance.sizeZ) * Maze.instance.MS);

		// create exit portal on random room on top floor
		if (exitPortal == null) {
			exitPortal = Instantiate (exitPortalPrefab);
		}
		exitPortal.transform.position = new Vector3(Random.Range (0, Maze.instance.sizeX) * Maze.instance.MS, (Maze.instance.yFloors-1) * Maze.instance.MS, Random.Range (0, Maze.instance.sizeZ) * Maze.instance.MS);

		// create NumKeys in random rooms on random floors
		for (int i = 0; i < NumKeys; i++) {
			GameObject key = Instantiate (portalKeysPrefab);
			key.transform.position = new Vector3(Random.Range (0, Maze.instance.sizeX) * Maze.instance.MS, Random.Range (0, Maze.instance.yFloors) * Maze.instance.MS, Random.Range (0, Maze.instance.sizeZ) * Maze.instance.MS);
		}

		StartCoroutine (ActivatePlayer());
	}

	IEnumerator ActivatePlayer() {
		float iFade = 1;
		Color fadeColor;

		while (iFade > 0) {
			iFade -= Time.deltaTime / 3;
			fadeColor = nextLevel.color;
			fadeColor.a = iFade;
			nextLevel.color = fadeColor;
			yield return null;
		}

		nextLevelText.enabled = false;
		playerActive = true;
	}

	public IEnumerator FadeOut() {
		float iFade = 0;
		Color fadeColor;

		playerActive = false;
		nextLevelText.text = "Level Complete!\n\nMoving to " + Maze.instance.sizeX.ToString () + "X" + Maze.instance.sizeZ.ToString () + "X" + Maze.instance.yFloors.ToString () + " structure.";
		nextLevelText.enabled = true;

		while (iFade < 1) {
			iFade += Time.deltaTime / 3;
			fadeColor = nextLevel.color;
			fadeColor.a = iFade;
			nextLevel.color = fadeColor;
			yield return null;
		}

		PlayerSetup ();
	}
	
	void Update () {
		if (playerActive) {
			// Mouse Look
			rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
			rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;

			if (rotationX < -360F)
				rotationX += 360F;
			if (rotationX > 360F)
				rotationX -= 360F;
			if (rotationY < -360F)
				rotationY += 360F;
			if (rotationY > 360F)
				rotationY -= 360F;

			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);

			transform.localRotation = originalRotation * xQuaternion * yQuaternion;

			// Player Movement
			if (Input.GetMouseButton (0)) {
				transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
			} else if (Input.GetMouseButton (1)) {
				transform.Translate (-Vector3.forward * moveSpeed * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.Escape)) {
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}
		
	public void UpdateUI(int floor, bool newRoom) {
		if (newRoom) {
			roomsVisited [floor] += 1;
		}
		floorNumber.text = (floor+1).ToString ();
		floorExplored.fillAmount = (float)roomsVisited [floor] / (float)roomsPerFloor;
		mazeExplored.fillAmount = (float)SumArray (roomsVisited) / (float)totalRooms;
	}

	private int SumArray(int[] toBeSummed)
	{
		int sum = 0;
		foreach (int item in toBeSummed)
		{
			sum += item;
		}
		return sum;
	}

	public void KeyPickup() {
		keysfound++;
		keyNumber.text = keysfound.ToString () + " of " + NumKeys.ToString ();

		if (keysfound >= NumKeys) {
			exitPortal.GetComponent<ExitPortal> ().ActivatePortal ();
		}
	}
}
