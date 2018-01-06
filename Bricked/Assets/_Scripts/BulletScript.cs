using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public Vector2 direction;
	public float startVelocity;
	public float deathMagnitude;
	public int damage;

	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.AddForce (direction * startVelocity, ForceMode2D.Impulse);
	}
	
	void Update () {
		if (rb.velocity.magnitude <= deathMagnitude) {
			Destroy (gameObject);
		}
	}
}
