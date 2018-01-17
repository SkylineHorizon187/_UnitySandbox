using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankInit : NetworkBehaviour {

	[SyncVar] public Color playerColor;
	[SyncVar] public string playerName;

	public GameObject fireParticleSystem;
	public GameObject bulletPrefab;
	private Transform firePoint;

	private float turretAngle;
	private PowerShot ps;

	public override void OnStartLocalPlayer() {
		firePoint = transform.GetChild (0);
		ps = FindObjectOfType <PowerShot> ();
	}

	public override void OnStartClient() {
		SpriteRenderer turretsr = GetComponent<SpriteRenderer> ();
		turretsr.color = playerColor;

		if (playerColor == Color.blue) {
			Transform p = GameObject.Find ("Player1Tank").transform;
			transform.parent = p;

			transform.localPosition = new Vector3 (4.8f, 3f, 0f);
			transform.localRotation = Quaternion.Euler (0f, 0f, 45f);
			turretAngle = 45f;
		} else {
			Transform p = GameObject.Find ("Player2Tank").transform;
			transform.parent = p;

			transform.localPosition = new Vector3 (-4.8f, 3f, 0f);
			transform.localRotation = Quaternion.Euler (0f, 0f, 135f);
			turretAngle = 135f;
		}
	}
	
	void Update () {
		if (!isLocalPlayer)
			return;

		// Get/Set turret angle
		float scroll = Input.GetAxis ("Mouse ScrollWheel") * 8;

		if (playerColor == Color.blue) {
			turretAngle += scroll;
			if (turretAngle < 0)
				turretAngle = 0;
			if (turretAngle > 90)
				turretAngle = 90;
		} else {
			turretAngle -= scroll;
			if (turretAngle < 90)
				turretAngle = 90;
			if (turretAngle > 180)
				turretAngle = 180;
		}

		transform.rotation = Quaternion.Euler (0,0, turretAngle);

		// Fire bullet
		if (Input.GetMouseButtonDown (0) && ps.isActive) {
			CmdFireBullet (firePoint.transform.position, firePoint.transform.rotation, ps.defaultPwr + ps.ShootAndReload ());
			// Particle system for gun barrel
			Instantiate (fireParticleSystem, firePoint.position, Quaternion.identity);
		}
	}

	[Command]
	void CmdFireBullet(Vector3 fromPos, Quaternion rot, float pwr) {
		RpcFireBullet (fromPos, rot, pwr);
	}

	[ClientRpc]
	void RpcFireBullet(Vector3 fromPos, Quaternion rot, float pwr) {
		GameObject bullet = Instantiate (bulletPrefab, fromPos, rot);
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D> ();
		rb.AddForce (bullet.transform.right * pwr, ForceMode2D.Impulse);
	}
}
