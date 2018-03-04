using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motel6 : MonoBehaviour {

	public Material exploredMaterial;
	public int myFloor;

	private bool visited;
	private Collider myCollider;

	void Start () {
		visited = false;
	}
	
	void OnTriggerEnter (Collider other) {
		if (!visited) {
			PaintTheWalls ();
			other.GetComponent<PlayerMove> ().UpdateUI (myFloor, true);
			visited = true;
		} else {
			other.GetComponent<PlayerMove> ().UpdateUI (myFloor, false);
		}
	}

	void PaintTheWalls() {
		Renderer[] wallRenderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in wallRenderers) {
			r.material = exploredMaterial;
		}
	}
}
