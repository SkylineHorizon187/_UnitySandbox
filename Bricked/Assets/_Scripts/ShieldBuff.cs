using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : MonoBehaviour {

	public BuffType buffType;

	private float lifeTime = 20f;

	void Start () {
	}

	void Update() {
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			shieldHolder.SH.AddCharge ();
			Destroy (gameObject);
		}
	}
}
