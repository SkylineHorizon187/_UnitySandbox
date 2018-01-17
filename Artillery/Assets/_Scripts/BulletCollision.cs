using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {
	public GameObject groundImpactPS;
	public GameObject tankImpactPS;

	private Vector2 windDir;
	private int windSpeed;
	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D> ();
		windDir = (FindObjectOfType<Clouds> ().windDirection == 1) ? Vector2.right : -Vector2.right;
		windSpeed = FindObjectOfType<Clouds> ().windSpeed;
	}

	void FixedUpdate () {
		// apply wind force to bullet
		rb.AddForce (windDir * windSpeed);
		Vector2 pos = transform.position;

		// Bullet Height test - Destroy if below ground level
		if (pos.y < -3 || pos.x < 0 || pos.x > AllTerrain.AT.mapWidth) {
			Destroy (gameObject);
		} else if (PixelLogic.HitTest (pos, 1)) {
			PixelLogic.CarveOutCircle (pos, 8);
			Destroy (gameObject);
			Instantiate (groundImpactPS, pos, Quaternion.identity);
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Player") {
			Instantiate (tankImpactPS, transform.position, Quaternion.identity);
			Destroy (other.gameObject);
			Destroy (gameObject);
		} 
	}
}
