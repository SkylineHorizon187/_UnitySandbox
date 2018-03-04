using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCell : MonoBehaviour {

	void Update () {
		transform.Rotate(0, Time.deltaTime * 12f, 0, Space.World);
	}

	void OnTriggerEnter(Collider other) {
		other.GetComponent<PlayerMove> ().KeyPickup ();
		Destroy (gameObject);
	}
}
