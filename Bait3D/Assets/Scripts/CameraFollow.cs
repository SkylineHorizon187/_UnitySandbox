using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	private Transform Player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		offset = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (GameController.PlayerLives) {
			transform.position = Player.transform.position + offset;
		}
	}
}
