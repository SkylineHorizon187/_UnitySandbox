using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour {

	private bool portalActive = false;

	public void ActivatePortal() {
		ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem p in pss) {
			var main = p.main;
			main.startColor = Color.green;
		}
		portalActive = true;
	}

	public void DeactivatePortal() {
		ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem p in pss) {
			var main = p.main;
			main.startColor = Color.red;
		}
		portalActive = false;
	}

	void OnTriggerEnter(Collider other) {
		if (portalActive) {
			// Reset Maze
			Maze.instance.sizeX += 1;
			Maze.instance.sizeZ += 1;
			if (Maze.instance.sizeX > 7) {
				Maze.instance.sizeX -= 4;
				Maze.instance.sizeZ -= 4;
				Maze.instance.yFloors += 1;
			}

			StartCoroutine (PlayerMove.instance.FadeOut ());

			Maze.instance.newMaze ();

			// Reset Portal
			DeactivatePortal ();
		}
	}

}
