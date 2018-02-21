using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnParticles : MonoBehaviour {
	public static SpawnParticles instance;
	[SerializeField]
	public Color32[] colors;
	public Transform mainCanvas;
	public GameObject particlePrefab;
	public int numParticles;
	public TextMeshProUGUI turboNumber;
	public Image turboTime;
	public float speedIncrement;

	private int maxTurbos = 9;
	private int numTurbos;
	private float speedTime;

	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	void Update () {
		if (speedTime > 0) {
			speedTime -= Time.deltaTime;
			turboTime.fillAmount = speedTime / speedIncrement;
		} else {
			if (numTurbos > 0) {
				speedTime += speedIncrement;
				numTurbos--;
				turboNumber.text = numTurbos.ToString ();
			} else {
				speedTime = 0;
				Time.timeScale = 1;
			}
		}
		/*
		if (Input.GetMouseButtonDown (0)) {
			ParticleSystem.Burst pb = new ParticleSystem.Burst();
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
		if (numTurbos < maxTurbos) {
			if (speedTime > 0) {
				numTurbos++;
				turboNumber.text = numTurbos.ToString ();
			} else {
				speedTime += speedIncrement;
			}
			Time.timeScale = 6;
		}
	}
}
