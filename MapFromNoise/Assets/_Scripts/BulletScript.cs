using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float bulletSpeed;
	public int bulletRadius;
	public int explodeRadius;

	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D> ();
		rb.AddForce (transform.up * bulletSpeed, ForceMode2D.Impulse);
	}
	
	void Update () {
		Vector2 curPos = new Vector2 (transform.position.x, transform.position.y);

		if (PixelLogic.HitTest (curPos, bulletRadius)) {
			PixelLogic.CarveOutCircle (curPos, explodeRadius);
			Destroy (gameObject);
		}

	}
}
