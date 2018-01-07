using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBuff : MonoBehaviour {

	public BuffType buffType;
	public int increaseAmount;

	private SpriteRenderer sr;
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
			PlayerScript ps = other.gameObject.GetComponent<PlayerScript> ();

			if (buffType == BuffType.BulletDamage) {
				ps.bulletDamage += increaseAmount;

				Destroy (gameObject);
			}
		}
	}

}
