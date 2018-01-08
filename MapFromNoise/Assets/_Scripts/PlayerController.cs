using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject bulletPrefab;
	public float moveSpeed;
	public int collideRadius;

	private Animator anim;
	private Transform firePoint;

	void Start () {
		anim = GetComponent<Animator> ();
		firePoint = transform.Find ("FirePoint");
	}
	
	void Update () {
		// Movement - WASD
		Vector2 mPos = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
		if (mPos == Vector2.zero) {
			anim.enabled = false;
		} else {
			anim.enabled = true;
			Vector2 curPos = new Vector2 (transform.position.x, transform.position.y);
			Vector2 newPos = curPos + mPos * moveSpeed * Time.deltaTime;
			if (!PixelLogic.HitTest (newPos, collideRadius))
				transform.position = newPos;
		}

		// Rotation - Look at mouse
		var mouse = Input.mousePosition;
		var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
		var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle - 90);

		// Shoot
		if (Input.GetMouseButtonDown (0)) {
			Instantiate (bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
		}
	}
}
