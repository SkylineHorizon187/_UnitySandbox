using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Smack : MonoBehaviour {

	public Text helpText;

	private Rigidbody rb;
	private ConstantForce cf;
	private bool hitGround;
	private Vector3 camOffset;
	private Vector3 ballStart;
	private bool inFlight = false;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		cf = GetComponent<ConstantForce> ();
		camOffset = Camera.main.transform.position;
	}
	
	void Update () {

		if (Input.GetMouseButtonDown (1)) {
			transform.position = new Vector3 (0, .5f, 0);
		}

		if (inFlight) {
			if (rb.velocity.magnitude < .3f) {
				rb.velocity = Vector3.zero;
				helpText.text = "Ball few: " + Vector3.Distance (ballStart, transform.position).ToString ();
				inFlight = false;
			} else {
				if (hitGround) {
					cf.force = Vector3.zero;
				}
			}

			Camera.main.transform.position = transform.position + camOffset;
		}
	}

	void OnCollisionEnter(Collision other) {
		hitGround = true;
		rb.angularDrag = 5f;
	}

	public void TakeShot(float power, float accuracy) {
		ballStart = transform.position;
		rb.angularDrag = .05f;
		rb.velocity = (Vector3.forward * power + Vector3.up * 35);
		cf.force = new Vector3(accuracy*.2f, 0, 0);
		hitGround = false;
		inFlight = true;
	}
}
