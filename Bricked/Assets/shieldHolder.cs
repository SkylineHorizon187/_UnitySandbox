using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldHolder : MonoBehaviour {
	public static shieldHolder SH;

	public GameObject shieldChargePrefab;

	void Start() {
		SH = this;
	}

	public void AddCharge() {
		Instantiate (shieldChargePrefab, transform);
	}

	public void RemoveCharge() {
		if (GetCharges () > 0) {
			Destroy (transform.GetChild (0).gameObject);
		}
	}

	public int GetCharges() {
		return transform.childCount;
	}
}
