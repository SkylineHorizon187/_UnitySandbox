using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShot : MonoBehaviour {
	public GameObject powerSelector;
	public GameObject shotTimer;
	public float shotDelay;

	public bool isActive = false;
	public int minPwr;
	public int maxPwr;
	public int barSpeed;
	public int defaultPwr;

	private Vector2 moveDir = Vector2.right;
	private float shotMaxScale;
	private float shotCountdown;

	void Start () {
		powerSelector.transform.position = new Vector2 (minPwr, 0);
		shotMaxScale = maxPwr;
	}

	void Update () {
		if (isActive) {
			// Power Selector Movement
			Vector2 moveAmt = powerSelector.transform.localPosition;
			moveAmt += moveDir * barSpeed * Time.deltaTime;
			powerSelector.transform.localPosition = moveAmt;

			if (powerSelector.transform.localPosition.x > maxPwr) {
				powerSelector.transform.localPosition = new Vector2 (maxPwr, 0);
				moveDir = -Vector2.right;
			}
			if (powerSelector.transform.localPosition.x < minPwr) {
				powerSelector.transform.localPosition = new Vector2 (minPwr, 0);
				moveDir = Vector2.right;
			}
		} else {
			// Shot timer
			shotCountdown -= Time.deltaTime / shotCountdown;
			if (shotCountdown <= 0) {
				shotCountdown = 0;
				isActive = true;
			}
			shotTimer.transform.localScale = new Vector3 (shotMaxScale * (shotCountdown / shotDelay), 1, 1);
		}
	}

	public float ShootAndReload() {
		isActive = false;
		shotCountdown = shotDelay;
		shotTimer.transform.localScale = new Vector3 (shotMaxScale, 1, 1);

		return powerSelector.transform.localPosition.x + 50;
	}
}
