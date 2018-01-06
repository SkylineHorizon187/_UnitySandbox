using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBuff : MonoBehaviour {

	public BuffType buffType;
	public int increaseAmount;

	private SpriteRenderer sr;
	private float lifeTime = 20f;

	void Start () {
		sr = GetComponent < SpriteRenderer> ();

		if (buffType == BuffType.Current) {
			sr.color = Color.red;
		} else if (buffType == BuffType.Maximum) {
			sr.color = Color.green;
		}
	}

	void Update() {
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0) {
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			PlayerScript ps = other.gameObject.GetComponent<PlayerScript> ();

			if (buffType == BuffType.Current) {
				ps.currentHealth += increaseAmount;
				if (ps.currentHealth > ps.playerHealth) {
					ps.currentHealth = ps.playerHealth;
				}
			} else if (buffType == BuffType.Maximum) {
				ps.playerHealth += increaseAmount;
			}
			ps.UpdateHealthGlobe ();

			Destroy (gameObject);
		}
	}
}
