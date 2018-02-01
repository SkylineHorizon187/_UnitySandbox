using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
	public float playerMoveSpeed;
	public float playerTurnSpeed;
	public Transform playerTrans;

	private Vector3 isoForward, isoRight;

	void Start () {
		isoForward = Camera.main.transform.forward.normalized;
		isoForward.y = 0;
		isoRight = Quaternion.Euler (new Vector3 (0, 90, 0)) * isoForward;
		playerTrans.forward = (isoForward + isoRight).normalized;
	}
	
	void Update () {
		Vector3 moveDir = (isoRight * Input.GetAxisRaw ("Horizontal") + isoForward * Input.GetAxisRaw ("Vertical")).normalized;

		if (moveDir != Vector3.zero) {
			playerTrans.rotation = Quaternion.RotateTowards (playerTrans.rotation, Quaternion.LookRotation (moveDir), playerTurnSpeed);
			transform.Translate (moveDir * playerMoveSpeed * Time.deltaTime);
		}
	}
}
