using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour {
	public static SpawnParticles instance;
	[SerializeField]
	public Color32[] colors;
	public Transform mainCanvas;
	public GameObject particlePrefab;
	public int numParticles;

	private ParticleSystem.Burst pb;
	private float speedTime;

	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	void Start () {
		pb = new ParticleSystem.Burst();
	}

	void Update () {
		speedTime -= Time.deltaTime;
		if (speedTime <= 0) {
			speedTime = 0;
			Time.timeScale = 1;
		}
		/*
		if (Input.GetMouseButtonDown (0)) {
			GameObject partSys = Instantiate (particlePrefab, transform.position, Quaternion.identity);
			pb.time = 0;
			pb.count = 25;
			pb.cycleCount = 1;
			pb.repeatInterval = .010f;
			partSys.GetComponent<ParticleSystem> ().emission.SetBurst (0, pb);
			Destroy (partSys, 16f);
		}
			*/
	}

	public void AddSpeedTime() {
		speedTime += 30;
		Time.timeScale = 6;
	}
}
